using System.Collections;
using UnityEngine;
using Bang.StateMachine;
using Bang.StateMachine.PlayerMachine;



public class TestPlayer : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float anchorOffset = 0.5f;
    [SerializeField] float groundDetectRadius;
    [SerializeField] float groundDetectDistance;
    [Space(5)]

    [Header("PlayerData")]
    [SerializeField] ObjectData playerData;

    public StateMachine<TestPlayer, ObjectData> stateMachine { get; private set; }
    public IdleState idleState { get; private set; }
    public MoveState moveState { get; private set; }


    private InputMaster _inputActions;
    private Rigidbody _rb;
    private float _flyTime = 0f;
    private Vector3 _gravity = new Vector3(0, -50f, 0);
    private float _gravityScale = 1f;

    private void Awake()
    {
        _inputActions = new InputMaster();
        _inputActions.Enable();

        stateMachine = new StateMachine<TestPlayer, ObjectData>();
        idleState = new IdleState(this, stateMachine, playerData);
        moveState = new MoveState(this, stateMachine, playerData);
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        stateMachine.Initalize(idleState);
    }
    private void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    #region State Callbacks

    #region Idle
    private void EnterIdle()
    {
        Debug.Log("Play Idle Animation.");
    }
    #endregion

    #region Jump
    private void EnterJump()
    {
        var downVelocity = Mathf.Min(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, jumpSpeed - downVelocity, 0);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
        _flyTime = 0f;
    }
    private void UpdateJump()
    {
        _flyTime += Time.deltaTime;
    }
    private void ExitJump()
    {
        // 產生剛好的力道來抵銷上升的動量
        var upVelocity = Mathf.Max(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, -upVelocity, 0);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
    }

    #endregion

    #region Fall
    private void EnterFall()
    {
        Debug.Log("Falling.");
    }
    #endregion

    #endregion
}