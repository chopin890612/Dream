using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    #region Player Input
    public Vector2 Movement { get; private set; }
    public Action<InputArgs> OnJumpPressed;
    public Action<InputArgs> OnJumpReleased;
    public Action<InputArgs> OnDash;
    public Action<InputArgs> OnAttack;
    #endregion

    #region UI Input
    public Vector2 UIMovment { get; private set; }
    public Action<InputArgs> OnSelect;
    public Action<InputArgs> OnCancle;
    public Action<InputArgs> OnRotate;
    #endregion

    private InputMaster playerAction;
    private InputMaster UIAction;

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

        playerAction = new InputMaster();
        UIAction = new InputMaster();

        #region Assign Input
        playerAction.Player.Movment.performed += ctx => Movement = ctx.ReadValue<Vector2>();
        playerAction.Player.Movment.canceled += ctx => Movement = Vector2.zero;

        playerAction.Player.Jump.performed += ctx => OnJumpPressed(new InputArgs { context = ctx});
        playerAction.Player.JumpUp.performed += ctx => OnJumpReleased(new InputArgs { context = ctx });
        playerAction.Player.Dash.performed += ctx => OnDash(new InputArgs { context = ctx });
        playerAction.Player.Attack.performed += ctx => OnAttack(new InputArgs { context = ctx });


        UIAction.PlantModule.Movment.performed += ctx => UIMovment = ctx.ReadValue<Vector2>();
        UIAction.PlantModule.Movment.canceled += ctx => UIMovment = Vector2.zero;

        UIAction.PlantModule.Confirm.performed += ctx => OnSelect(new InputArgs { context = ctx });
        UIAction.PlantModule.Cancle.performed += ctx => OnCancle(new InputArgs { context = ctx });
        UIAction.PlantModule.Rotate.performed += ctx => OnRotate(new InputArgs { context = ctx });
        #endregion
    }

    public class InputArgs
    {
        public InputAction.CallbackContext context;
    }

    public void SetActionEnable(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.GameView)
        {
            playerAction.Enable();
            UIAction.Disable();
        }
        else if(gameState == GameManager.GameState.GameMenu)
        {
            playerAction.Disable();
            UIAction.Enable();
        }
    }
}
