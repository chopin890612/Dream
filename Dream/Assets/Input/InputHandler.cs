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
    public Action<InputArgs> OnUIPressUp;
    public Action<InputArgs> OnUIPressDown;
    public Action<InputArgs> OnUIPressLeft;
    public Action<InputArgs> OnUIPressRight;
    public Action<InputArgs> OnUISelect;
    public Action<InputArgs> OnUICancle;
    public Action<InputArgs> OnUIRotate;
    #endregion

    #region Dialogue Input
    public Action<InputArgs> OnPressUp;
    public Action<InputArgs> OnPressDown;
    public Action<InputArgs> OnSelect;
    #endregion

    private InputMaster playerAction;
    private InputMaster UIAction;
    private InputMaster DialogueAction;

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
        DialogueAction = new InputMaster();

        #region Assign Input
        playerAction.Player.Movment.performed += ctx => Movement = ctx.ReadValue<Vector2>();
        playerAction.Player.Movment.canceled += ctx => Movement = Vector2.zero;

        playerAction.Player.Jump.performed += ctx => OnJumpPressed(new InputArgs { context = ctx});
        playerAction.Player.JumpUp.performed += ctx => OnJumpReleased(new InputArgs { context = ctx });
        playerAction.Player.Dash.performed += ctx => OnDash(new InputArgs { context = ctx });
        playerAction.Player.Attack.performed += ctx => OnAttack(new InputArgs { context = ctx });


        UIAction.PlantModule.Movment.performed += ctx => UIMovment = ctx.ReadValue<Vector2>();
        UIAction.PlantModule.Movment.canceled += ctx => UIMovment = Vector2.zero;
        UIAction.PlantModule.UpPress.performed += ctx => OnUIPressUp(new InputArgs { context = ctx });
        UIAction.PlantModule.DownPress.performed += ctx => OnUIPressDown(new InputArgs { context = ctx });
        UIAction.PlantModule.LeftPress.performed += ctx => OnUIPressLeft(new InputArgs { context = ctx });
        UIAction.PlantModule.RightPress.performed += ctx => OnUIPressRight(new InputArgs { context = ctx });

        UIAction.PlantModule.Confirm.performed += ctx => OnUISelect(new InputArgs { context = ctx });
        UIAction.PlantModule.Cancle.performed += ctx => OnUICancle(new InputArgs { context = ctx });
        UIAction.PlantModule.Rotate.performed += ctx => OnUIRotate(new InputArgs { context = ctx });

        DialogueAction.PlantModule.UpPress.performed += ctx => OnPressUp(new InputArgs { context = ctx });
        DialogueAction.PlantModule.DownPress.performed += ctx => OnPressDown(new InputArgs { context = ctx });
        DialogueAction.PlantModule.Confirm.performed += ctx => OnSelect(new InputArgs { context = ctx });
        #endregion
    }

    public class InputArgs
    {
        public InputAction.CallbackContext context;
    }

    public void SetActionEnable(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.GameView:
                playerAction.Enable();
                UIAction.Disable();
                DialogueAction.Disable();
                break;
            case GameManager.GameState.GameMenu:
                playerAction.Disable();
                UIAction.Enable();
                DialogueAction.Disable();
                break;
            case GameManager.GameState.Dialogue:
                playerAction.Disable();
                UIAction.Disable();
                DialogueAction.Enable();
                break;
        }
    }
}
