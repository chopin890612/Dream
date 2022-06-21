using System.Collections;
using UnityEngine;
using Bang.State;


public class TestPlayer : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float anchorOffset = 0.5f;
    [SerializeField] float groundDetectRadius;
    [SerializeField] float groundDetectDistance;


    private InputMaster _inputActions;
    private StateMachine _stateMachine;
    private Rigidbody _rb;
    private float _flyTime = 0f;
    private Vector3 _gravity = new Vector3(0, -50f, 0);
    private float _gravityScale = 1f;

    private void Awake()
    {
        _inputActions = new InputMaster();
        _inputActions.Enable();
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        
        State idle = new State(EnterIdle, null, null);
        State jump = new State(EnterJump, UpdateJump, ExitJump);
        State fall = new State(EnterFall, null, null);

        idle.When( () => _inputActions.Player.Jump.ReadValue<float>() == 1f, jump, null);
        jump.When(() => _flyTime > 0.5f, fall, null);
        jump.When(() => _inputActions.Player.Jump.ReadValue<float>() == 0f, fall, null);
        fall.When(() => Physics.SphereCast(transform.position + new Vector3(0, anchorOffset, 0), groundDetectRadius, Vector3.down, 
            out RaycastHit hit, groundDetectDistance, LayerMask.GetMask("Ground")), idle, null);


        _stateMachine = new StateMachine(idle);
        _stateMachine.AddStates(idle, jump, fall);
    }
    private void Update()
    {
        _rb.AddForce(_gravity * _gravityScale, ForceMode.Acceleration);
        _stateMachine.ExcuteActions(_stateMachine.Tick());
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