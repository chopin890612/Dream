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
    public Action<InputArgs> OnChangeWorld;
    public Action<InputArgs> ReleaseChangeWorld;
    public Action<InputArgs> OnFire;
    #endregion

    #region UI Input
    public Vector2 UIMovment { get; private set; }
    public Action<InputArgs> OnUIConfirm;
    public Action<InputArgs> OnUIBack;
    #endregion

    #region Dialogue
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
        playerAction.Player.ChangeWorld.performed += ctx => OnChangeWorld(new InputArgs { context = ctx });
        playerAction.Player.ChangeWorld.canceled += ctx => ReleaseChangeWorld(new InputArgs { context = ctx });
        playerAction.Player.Fire.performed += ctx => OnFire(new InputArgs { context = ctx });


        UIAction.UI.Move.performed += ctx => UIMovment = ctx.ReadValue<Vector2>();
        UIAction.UI.Move.canceled += ctx => UIMovment = Vector2.zero;

        UIAction.UI.Confirm.performed += ctx => OnUIConfirm(new InputArgs { context = ctx });
        UIAction.UI.Back.performed += ctx => OnUIBack(new InputArgs { context = ctx });


        DialogueAction.Dialogue.Up.performed += ctx => OnPressUp(new InputArgs { context = ctx });
        DialogueAction.Dialogue.Down.performed += ctx => OnPressDown(new InputArgs { context = ctx });
        DialogueAction.Dialogue.Confirm.performed += ctx => OnSelect(new InputArgs { context = ctx });
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
