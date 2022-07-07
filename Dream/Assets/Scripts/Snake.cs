using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class Snake : MonoBehaviour, IStateMachine
{
    [Header("X Axis Movement")]
    [SerializeField] float moveSpeed = 25f;
    [SerializeField] bool faceRight = true;
    [Space(5)]

    [Header("Y Axis Movement")]
    [SerializeField] float jumpSpeed = 45f;
    [SerializeField] float climbSpeed = 45f;
    [SerializeField] float fallSpeedLimiter = 45f;
    [Space(5)]

    [Header("Ground Settings")]
    [SerializeField] float anchorOffset = 1f;
    [SerializeField] float groundDetectRadius = 0.2f;
    [SerializeField] float groundDetectDistance = 0.5f;
    [SerializeField] bool isWalking = false;
    [SerializeField] bool onGround = false;
    [SerializeField] LayerMask groundDetectLayer;
    [Space(5)]

    [Header("Wall Settings")]
    [SerializeField] float wallDetectDistance = 1f;
    [SerializeField] Vector3 wallDetectRadius = new Vector3(1, 1, 1);
    [SerializeField] bool onWall = false;
    [SerializeField] bool isWallJumping = false;
    [SerializeField] float wallJumpSpeedx;
    [SerializeField] LayerMask wallDetectLayer;
    [Space(5)]

    [Header("Dialogue")]
    [SerializeField] GameObject chatWindow;

    private InputMaster _inputActions;
    private bool _jumpButton;
    [SerializeField]private Rigidbody _rb;
    private SkeletonAnimation _skeletonAnimation;
    private Animator _animator;
    [SerializeField] private bool _isWallOnRight = true;
    private Transform _wallTransform;
    private float _playerDirection;
    private float _climbDirection;
    private Vector3 _gravity = new Vector3(0, -50f, 0);
    private float _gravityScale = 1f;
    private bool _dashButton;

    private float currentSphereDistance;
    private float currentBoxDistance;

    private void Start()
    {
        _inputActions = new InputMaster();
        _inputActions.Enable();
        _animator = GetComponent<Animator>();
        //_rb = GetComponent<Rigidbody>();
        //_skeletonAnimation = GetComponent<SkeletonAnimation>();
    }
    private void Update()
    {
        PlayerFlip();
        _playerDirection = _inputActions.Player.Movment.ReadValue<float>();
        _climbDirection = _inputActions.Player.Climb.ReadValue<float>();

        _animator.SetBool("OnWall", onWall);

        _jumpButton = _inputActions.Player.Jump.ReadValue<float>() == 1f ? true : false;
        _animator.SetBool("JumpButton", _jumpButton);

        _animator.SetBool("IsWallJumping", isWallJumping);
        
        _dashButton = _inputActions.Player.Dash.ReadValue<float>() == 1f ? true : false;
    }
    private void FixedUpdate()
    {
        _rb.AddForce(_gravity * _gravityScale, ForceMode.Acceleration);
        if (_rb.velocity.y < fallSpeedLimiter)
            _rb.velocity = new Vector2(_rb.velocity.x, fallSpeedLimiter);

        //Ground Update
        onGround = Physics.SphereCast(transform.position + new Vector3(0, anchorOffset, 0), groundDetectRadius, Vector3.down,
            out RaycastHit hitGround, groundDetectDistance, groundDetectLayer);
        if (!isWallJumping)
        {
            _rb.velocity = new Vector2(_playerDirection * moveSpeed, _rb.velocity.y);
        }

        //Wall Update
        onWall = Physics.BoxCast(transform.position + new Vector3(0, anchorOffset, 0), wallDetectRadius / 2, faceRight ? Vector3.right : Vector3.left,
                 out RaycastHit hitwall, Quaternion.identity, wallDetectDistance, wallDetectLayer);
        if (onWall)
        {
            _wallTransform = hitwall.transform;
            WallFace();
            _rb.velocity = new Vector2(0, _climbDirection * climbSpeed);
            if(_rb.velocity.y != 0f)
            {
                GameManager.instance.SetControllerVibration(0.2f, 0.5f, 0.1f);
            }
        }

        //Debugger
        if (hitGround.distance == 0f)
            currentSphereDistance = groundDetectDistance;
        else
            currentSphereDistance = hitGround.distance;

        if (hitwall.distance == 0f)
            currentBoxDistance = wallDetectDistance;
        else
            currentBoxDistance = hitwall.distance;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -currentSphereDistance + anchorOffset, 0), groundDetectRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(faceRight ? currentBoxDistance : -currentBoxDistance, anchorOffset), wallDetectRadius);
    }
    private void WallFace()
    {
        if ((_wallTransform.position - transform.position).x > 0)
            _isWallOnRight = true;
        else if ((_wallTransform.position - transform.position).x < 0)
            _isWallOnRight = false;
    }
    private void PlayerFlip()
    {
        if (_playerDirection > 0 && faceRight == false)
        {
            transform.Rotate(0, 180f, 0);
            faceRight = true;
        }
        else if (_playerDirection < 0 && faceRight == true)
        {
            transform.Rotate(0, -180f, 0);
            faceRight = false;
        }
    }
    public void StateCallback(string stateName)
    {
        switch (stateName)
        {
            case "OnWall":
                OnWall();
                break;
            case "ExitWall":
                ExitWall();
                break;
            case "StartClimbJump":
                StartClimbJump();
                break;
            case "EndClimbJump":
                EndClimbJump();
                break;
            case "ClimbJump":
                ClimbJump();
                break;
            default:
                Debug.LogWarning("Wrong state state name: " + stateName);
                break;
        }
    }

    //###################################################################################

    private void Jump()
    {
        var downVelocity = Mathf.Min(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, jumpSpeed - downVelocity, 0);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
    }
    private void EndJump()
    {   
        // 產生剛好的力道來抵銷上升的動量
        var upVelocity = Mathf.Max(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, -upVelocity, 0);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
    }
    private void OnWall()
    {
        _gravityScale = 0;
        _rb.velocity = Vector2.zero;
    }
    private void ExitWall()
    {
        _gravityScale = 1f;
    }
    private void StartClimbJump()
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(new Vector3(wallJumpSpeedx * (_isWallOnRight == true ? -1 : 1), 0, 0), ForceMode.Impulse);
        isWallJumping = true;
        Invoke("SetIsWallJumpingFalse", 0.15f);
        _gravityScale = 1;
    }
    private void ClimbJump()
    {
        Jump();
    }
    private void SetIsWallJumpingFalse()
    {
        isWallJumping = false;
    }
    private void EndClimbJump()
    {
        EndJump();
    }
}
