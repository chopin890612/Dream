using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;
    public Vector2 Movement { get; private set; }

    public Action<InputArgs> OnJumpPressed;
    public Action<InputArgs> OnJumpReleased;
    public Action<InputArgs> OnDash;
    public Action<InputArgs> OnAttack;

    private InputMaster inputActions;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        inputActions = new InputMaster();

        #region Assign Input
        inputActions.Player.Movment.performed += ctx => Movement = ctx.ReadValue<Vector2>();
        inputActions.Player.Movment.canceled += ctx => Movement = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => OnJumpPressed(new InputArgs { context = ctx});
        inputActions.Player.JumpUp.performed += ctx => OnJumpReleased(new InputArgs { context = ctx });
        inputActions.Player.Dash.performed += ctx => OnDash(new InputArgs { context = ctx });
        inputActions.Player.Attack.performed += ctx => OnAttack(new InputArgs { context = ctx });
        #endregion
    }

    public class InputArgs
    {
        public InputAction.CallbackContext context;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
}
