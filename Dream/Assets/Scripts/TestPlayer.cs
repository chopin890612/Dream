using System.Collections;
using UnityEngine;
using Bang.StateMachine;
using Bang.StateMachine.PlayerMachine;



public class TestPlayer : MonoBehaviour
{
    #region State Variables

    public StateMachine<TestPlayer, PlayerData> stateMachine { get; private set; }
    public IdleState idleState { get; private set; }
    public MoveState moveState { get; private set; }
    public JumpState jumpState { get; private set; }
    public AirState airState { get; private set; }
    public LandState landState { get; private set; }

    [SerializeField] PlayerData playerData;
    #endregion

    #region Components
    public InputHandler _inputActions { get; private set; }
    public Rigidbody _rb { get; private set; }
    #endregion

    public int _faceDirection = 1;
    public float _gravityScale = 1f;
    public bool G;
    private float groundDistance;

    #region Unity LifeCycle

    private void Awake()
    {
        stateMachine = new StateMachine<TestPlayer, PlayerData>();
        idleState = new IdleState(this, stateMachine, playerData);
        moveState = new MoveState(this, stateMachine, playerData);
        jumpState = new JumpState(this, stateMachine, playerData);
        airState = new AirState(this, stateMachine, playerData);
        landState = new LandState(this, stateMachine, playerData);
    }
    private void Start()
    {
        _inputActions = GetComponent<InputHandler>();
        _rb = GetComponent<Rigidbody>();
        stateMachine.Initalize(idleState);
    }
    private void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        UseGravity(_gravityScale);
        stateMachine.currentState.PhysicsUpdate();
        
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        G = CheckOnGround();
        Gizmos.color = Color.yellow;
        if(groundDistance > 0f && groundDistance < playerData.groundDetectDistance)
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, playerData.anchorOffset + -groundDistance, 0), playerData.groundDetectRadius);
        else
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, playerData.anchorOffset + -playerData.groundDetectDistance, 0), playerData.groundDetectRadius);
    }

    public void Jump()
    {
        var downVelocity = Mathf.Min(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, playerData.jumpSpeed - downVelocity, 0);
        _rb.velocity = deltaVelocity;
    }
    public void EndJump()
    {
        var upVelocity = Mathf.Max(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, -upVelocity, 0);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
    }
    public void XMovement(float xMove)
    {
        _rb.velocity = new Vector2(xMove * playerData.moveSpeed, _rb.velocity.y);
    }
    public void UseGravity(float gravityScale)
    {
        _rb.AddForce(playerData.gravity * gravityScale, ForceMode.Acceleration);
    }
    public void CheckIfShouldFlip()
    {
        if (_inputActions.NormInputX != 0 && _inputActions.NormInputX != _faceDirection)
            Flip();
    }
    public bool CheckOnGround()
    {
        var onGround = Physics.SphereCast(transform.position + new Vector3(0, playerData.anchorOffset, 0), playerData.groundDetectRadius, Vector3.down,
                out RaycastHit hitGround, playerData.groundDetectDistance, LayerMask.GetMask("Ground"));
        groundDistance = hitGround.distance;

        return onGround;
    }
    private void Flip()
    {
        _faceDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
}
    