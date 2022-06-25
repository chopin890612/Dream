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
    public float JumpButtonStartTime { get; private set; }

    private InputMaster inputActions;

    private void Awake()
    {
        inputActions = new InputMaster();
        inputActions.Enable();
    }
    private void Start()
    {
        inputActions.Player.Jump.started += StartJump;
        inputActions.Player.Jump.canceled += CanceledJump;
        inputActions.Player.Jump.performed += PerformedJump;
    }
    private void Update()
    {
        rawMove = inputActions.Player.Movment.ReadValue<float>();
        NormInputX = (int)(rawMove * Vector2.right).normalized.x;
        //
        //JumpButton = inputActions.Player.Jump.ReadValue<float>() == 1f ? true : false;
    }
    private void StartJump(InputAction.CallbackContext context)
    {
        JumpButtonStartTime = Time.time;
        JumpButton = true;
    }
    private void CanceledJump(InputAction.CallbackContext context)
    {
        JumpButton = false;
    }
    private void PerformedJump(InputAction.CallbackContext context)
    {
        //isJumped = true;
    }
}
