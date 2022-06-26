using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchShape : MonoBehaviour
{
    [SerializeField] GameObject Human;
    [SerializeField] GameObject Snake;
    [SerializeField] ParticleSystem pp;
    private InputMaster _inputAction; 


    void Start()
    {
        _inputAction = new InputMaster();
        _inputAction.Enable();
        _inputAction.Player.ShapeShift.started += ShapeShift;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShapeShift(InputAction.CallbackContext context)
    {
        Human.SetActive(!Human.activeSelf);
        Snake.SetActive(!Snake.activeSelf);
        pp.Play();
    }
}
