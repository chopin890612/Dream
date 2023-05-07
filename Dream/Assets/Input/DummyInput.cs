using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DummyInput : MonoBehaviour
{
    public Vector2 dummyMovment = Vector2.zero;

    [Header("Press")]
    public InputAction press;
    [Header("Enable")]
    public InputAction EDummy;
    [Header("Disable")]
    public InputAction DDummy;

    [SerializeField]private bool enableDummy = false;

    private void Awake()
    {
        press.Enable();
        EDummy.Enable();
        DDummy.Enable();
    }
    private void Start()
    {
        press.performed += ctx => DummyJump();
        EDummy.performed += ctx => EnableDummy();
        DDummy.performed += ctx => DisableDummy();
    }
    public void EnableDummy()
    {
        InputHandler.instance.DisconnectInput();

        enableDummy = true;
    }
    public void DisableDummy()
    {
        InputHandler.instance.ConnectInput();

        enableDummy = false;
    }
    public void DummyJump()
    {
        //InputHandler.instance.OnJumpPressed.Invoke(new InputHandler.InputArgs());
        GameManager.instance.player.Jump();
    }
    public void DummyFall()
    {
        InputHandler.instance.OnJumpReleased.Invoke(new InputHandler.InputArgs());
    }
    public void DummyDash()
    {
        InputHandler.instance.OnDash.Invoke(new InputHandler.InputArgs());
    }
    private void Update()
    {
        if(enableDummy)
            InputHandler.instance.dummyMovment = dummyMovment;
    }
}
