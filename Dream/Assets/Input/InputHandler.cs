using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    [SerializeField] private bool enableDummyMove = false;
    public Vector2 dummyMovment = Vector2.zero;

    #region Player Input
    public Vector2 Movement { get; private set; }
    public Action<InputArgs> OnJumpPressed;
    public Action<InputArgs> OnJumpReleased;
    public Action<InputArgs> OnDash;
    public Action<InputArgs> OnAttack;
    public Action<InputArgs> OnChangeWorld;
    public Action<InputArgs> ReleaseChangeWorld;
    public Action<InputArgs> OnFire;
    public Action<InputArgs> OnInteract;
    public Action<InputArgs> OnPause;
    #endregion

    #region UI Input
    public Vector2 UIMovment { get; private set; }
    public Action<InputArgs> OnUIConfirm;
    public Action<InputArgs> OnUIBack;
    public Action<InputArgs> OnUIPause;
    #endregion

    #region Dialogue
    public Action<InputArgs> OnPressUp;
    public Action<InputArgs> OnPressDown;
    public Action<InputArgs> OnSelect;
    #endregion

    #region Prologue
    public Vector2 PMovment { get; private set; }
    public Action<InputArgs> OnPConfirm;
    public Action<InputArgs> OnPBack;
    #endregion

    private InputMaster playerAction;
    private InputMaster UIAction;
    private InputMaster DialogueAction;
    private InputMaster PrologueAction;

    private Action<InputAction.CallbackContext> DashHandler;
    private Action<InputAction.CallbackContext> JumpPressHandler;
    private Action<InputAction.CallbackContext> JumpReleaseHandler;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);

        playerAction = new InputMaster();
        UIAction = new InputMaster();
        DialogueAction = new InputMaster();
        PrologueAction = new InputMaster();
        #region Assign Input

        JumpPressHandler = ctx => OnJumpPressed(new InputArgs { context = ctx });
        JumpReleaseHandler = ctx => OnJumpReleased(new InputArgs { context = ctx });
        DashHandler = ctx => OnDash(new InputArgs { context = ctx });

        playerAction.Player.Movment.performed += ctx => Movement = ctx.ReadValue<Vector2>();
        playerAction.Player.Movment.canceled += ctx => Movement = Vector2.zero;
        playerAction.Player.Jump.performed += JumpPressHandler;
        playerAction.Player.JumpUp.performed += JumpReleaseHandler;
        playerAction.Player.Dash.performed += DashHandler;
        playerAction.Player.Attack.performed += ctx => OnAttack(new InputArgs { context = ctx });
        playerAction.Player.ChangeWorld.performed += ctx => OnChangeWorld(new InputArgs { context = ctx });
        playerAction.Player.ChangeWorld.canceled += ctx => ReleaseChangeWorld(new InputArgs { context = ctx });
        playerAction.Player.Fire.performed += ctx => OnFire(new InputArgs { context = ctx });
        playerAction.Player.Interact.performed += ctx => OnInteract(new InputArgs { context = ctx });
        playerAction.Player.Pause.performed += ctx => OnPause(new InputArgs { context = ctx });

        //------------------------------------------------------------------------------------------------------------------------

        UIAction.UI.Move.performed += ctx => UIMovment = ctx.ReadValue<Vector2>();
        UIAction.UI.Move.canceled += ctx => UIMovment = Vector2.zero;

        UIAction.UI.Confirm.performed += ctx => OnUIConfirm(new InputArgs { context = ctx });
        UIAction.UI.Back.performed += ctx => OnUIBack(new InputArgs { context = ctx });
        UIAction.UI.Pause.performed += ctx => OnUIPause(new InputArgs { context = ctx });

        //------------------------------------------------------------------------------------------------------------------------

        DialogueAction.Dialogue.Up.performed += ctx => OnPressUp(new InputArgs { context = ctx });
        DialogueAction.Dialogue.Down.performed += ctx => OnPressDown(new InputArgs { context = ctx });
        DialogueAction.Dialogue.Confirm.performed += ctx => OnSelect(new InputArgs { context = ctx });

        //------------------------------------------------------------------------------------------------------------------------

        PrologueAction.Prologue.Move.performed += ctx => PMovment = ctx.ReadValue<Vector2>();
        PrologueAction.Prologue.Move.canceled += ctx => PMovment = Vector2.zero;

        PrologueAction.Prologue.Confirm.performed += ctx => OnPConfirm(new InputArgs { context = ctx });
        PrologueAction.Prologue.Back.performed += ctx => OnPBack(new InputArgs { context = ctx });
        #endregion

        #region Dummy Action
        OnJumpPressed += ctx => Dummy();
        OnJumpReleased += ctx => Dummy();
        OnDash += ctx => Dummy();
        OnAttack += ctx => Dummy();
        OnChangeWorld += ctx => Dummy();
        ReleaseChangeWorld += ctx => Dummy();
        OnFire += ctx => Dummy();
        OnInteract += ctx => Dummy();
        OnPause += ctx => Dummy();


        OnUIConfirm += ctx => Dummy();
        OnUIBack += ctx => Dummy();
        OnUIPause += ctx => Dummy();


        OnPressUp += ctx => Dummy();
        OnPressDown += ctx => Dummy();
        OnSelect += ctx => Dummy();

        OnPConfirm += ctx => Dummy();
        OnPBack += ctx => Dummy();
        #endregion
    }

    private void Update()
    {
        if (enableDummyMove)
        {
            Movement = dummyMovment;
        }
    }
    private void DisconnectDash()
    {
        playerAction.Player.Dash.performed -= DashHandler;
    }
    private void DisconnectJump()
    {
        playerAction.Player.Jump.performed -= JumpPressHandler;
        playerAction.Player.JumpUp.performed -= JumpReleaseHandler;
    }
    public void DisconnectInput()
    {
        DisconnectDash();
        DisconnectJump();
        enableDummyMove = true;
        Debug.Log("Disconnect");
    }
    public void ConnectInput()
    {
        playerAction.Player.Dash.performed += DashHandler;
        Movement = playerAction.Player.Movment.ReadValue<Vector2>();
        playerAction.Player.Jump.performed += JumpPressHandler;
        playerAction.Player.JumpUp.performed += JumpReleaseHandler;
        enableDummyMove = false;
        Debug.Log("Connect");
    }
    private void Dummy() { }

    public class InputArgs
    {
        public InputAction.CallbackContext context;
    }

    public void SetActionEnable(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.MainMenu:
                playerAction.Disable();
                UIAction.Disable();
                DialogueAction.Disable();
                PrologueAction.Enable();
                break;
            case GameManager.GameState.Prologue:
                playerAction.Disable();
                UIAction.Disable();
                DialogueAction.Disable();
                PrologueAction.Enable();
                break;
            case GameManager.GameState.GameView:
                playerAction.Enable();
                UIAction.Disable();
                DialogueAction.Disable();
                PrologueAction.Disable();
                break;
            case GameManager.GameState.GameMenu:
                playerAction.Disable();
                UIAction.Enable();
                DialogueAction.Disable();
                PrologueAction.Disable();
                break;
            case GameManager.GameState.Dialogue:
                playerAction.Disable();
                UIAction.Disable();
                DialogueAction.Enable();
                PrologueAction.Disable();
                break;
            case GameManager.GameState.Loading:
                playerAction.Disable();
                UIAction.Disable();
                DialogueAction.Disable();
                PrologueAction.Disable();
                break;

            default:
                Debug.LogError("There is no " + gameState.ToString() + " GameState.");
                break;
        }
    }
}
