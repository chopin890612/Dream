using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public float rawMove { get; private set; }
    public int NormInputX { get; private set; }
    public bool JumpButton { get; private set; }

    private InputMaster inputActions;


    private void Awake()
    {
        inputActions = new InputMaster();
        inputActions.Enable();
    }
    private void Update()
    {
        rawMove = inputActions.Player.Movment.ReadValue<float>();
        NormInputX = (int)(rawMove * Vector2.right).normalized.x;

        JumpButton = inputActions.Player.Jump.ReadValue<float>() == 1f ? true : false;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        rawMove = context.ReadValue<float>();
        NormInputX = Mathf.RoundToInt(rawMove);
        Debug.Log("123");
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpButton = true;
        }
        if (context.canceled)
        {
            JumpButton = false;
        }
    }
}
