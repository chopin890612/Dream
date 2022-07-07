using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using XInputDotNetPure;

public class Player : MonoBehaviour, IStateMachine
{
    #region Variables

    #region Setting Variables
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
    [SerializeField] LayerMask groundDetectLayers;
    [Space(5)]

    [Header("Wall Settings")]
    [SerializeField] float wallDetectDistance = 1f;
    [SerializeField] Vector3 wallDetectRadius = new Vector3(1,1,1);
    [SerializeField] bool onWall = false;
    [SerializeField] bool isWallJumping = false;
    [SerializeField] float wallJumpSpeedx;
    [SerializeField] LayerMask wallDetectLayers;
    [Space(5)]

    [Header("Dash Settings")]
    [SerializeField] float dashCooldown = 0.5f;
    [SerializeField] float dashSpeed = 50f;
    [SerializeField] bool isDashing = false;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] bool canDash = false;
    [SerializeField] float nextDashTime;
    [Space(5)]

    [Header("Controller")]
    PlayerIndex playerIndex = PlayerIndex.One;
    GamePadState state;
    GamePadState prevState;
    #endregion

    #region Degugger
    [Header("Debugger")]
    [SerializeField] float currentSphereDistance;
    [SerializeField] float currentBoxDistance;
    [Space(5)]
    #endregion

    #region Animation
    [Header("Aniamtion")]
    [SerializeField] AnimationReferenceAsset walk;
    [SerializeField] AnimationReferenceAsset attack;
    [SerializeField] AnimationReferenceAsset idle;
    [SerializeField] AnimationReferenceAsset jump;
    #endregion

    #region Other Variables
    private bool _jumpButton;
    private Vector3 _gravity = new Vector3(0, -50f, 0);
    private float _gravityScale = 1f;
    private InputMaster _inputActions;
    private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    private SkeletonAnimation _skeletonAnimation;
    private Spine.EventData _endAttEvent;
    private Spine.EventData _onStepEvenet;
    private float _flyTime = 0f;
    private float _playerDirection;
    private GameObject _spineRenderer;
    private bool _isWallOnRight =true;
    private Transform _wallTransform;
    private bool _dashButton;
    #endregion

    #endregion

    #region Unity Functions
    private void Awake()
    {
        _inputActions = new InputMaster();
        _inputActions.Enable();
    }
    private void Start()
    {
        _spineRenderer = transform.GetChild(0).gameObject;
        _animator = GetComponent<Animator>();
        //_rb = GetComponent<Rigidbody>();
        _skeletonAnimation = _spineRenderer.GetComponent<SkeletonAnimation>();
    }
    private void Update()
    {
        _animator.SetBool("OnGround", onGround);
        _jumpButton = _inputActions.ShapeShifter.Jump.ReadValue<float>() == 1f ? true : false;
        _animator.SetBool("JumpButton", _jumpButton);
        
        _animator.SetFloat("FlyTime", _flyTime);
        _playerDirection = _inputActions.ShapeShifter.Movment.ReadValue<float>();

        isWalking = _playerDirection != 0f;
        _animator.SetBool("IsWalking", isWalking);

        _animator.SetBool("OnWall", onWall);

        _animator.SetBool("IsWallJumping", isWallJumping);

        _animator.SetBool("IsMoveToWall", _isWallOnRight);

        _dashButton = _inputActions.ShapeShifter.ChangeShape.ReadValue<float>() == 1f ? true : false;
        _animator.SetBool("DashButton", _dashButton);

        _animator.SetBool("IsDashing", isDashing);

        _animator.SetBool("CanDash", canDash);

        if (onGround == false)
            _flyTime += Time.deltaTime;
        if (onGround || onWall)
            if (!_dashButton)
                canDash = Time.time > nextDashTime;
        if (!isDashing)
        {
            if (isWalking)
                _skeletonAnimation.timeScale = Mathf.Abs(_playerDirection);
            if (!isWallJumping)
                _rb.velocity = new Vector3(_playerDirection * moveSpeed, _rb.velocity.y);
            PlayerFlip();
        }

        state = GamePad.GetState(playerIndex);
    }

    private void FixedUpdate()
    {
        //Custom Gravity
        _rb.AddForce(_gravity * _gravityScale, ForceMode.Acceleration);
        if (_rb.velocity.y < fallSpeedLimiter)
            _rb.velocity = new Vector2(_rb.velocity.x, fallSpeedLimiter);

        //Ground Update
        onGround = Physics.SphereCast(transform.position + new Vector3(0, anchorOffset, 0), groundDetectRadius, Vector3.down,
            out RaycastHit hitGround, groundDetectDistance, groundDetectLayers);
        if (!isDashing && !isWallJumping)
        {
            _rb.velocity = new Vector3(_playerDirection * moveSpeed, _rb.velocity.y);
        }

        //Wall Update
        onWall = Physics.BoxCast(transform.position + new Vector3(0, anchorOffset, 0), wallDetectRadius / 2, faceRight? Vector3.right : Vector3.left,
                 out RaycastHit hitwall, Quaternion.identity, wallDetectDistance, wallDetectLayers);
        if (onWall)
        {
            _wallTransform = hitwall.transform;
            WallFace();
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

    #endregion

    #region Other Function
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

    private void ShapeSwitchEvent(bool isSnake)
    {
        if (isSnake)
        {

        }
    }
    #endregion

    #region State Control
    public void StateCallback(string stateName)
    {
        switch (stateName)
        {
            //#################### Jump ############################
            case "StartJump":
                StartJump();
                break;
            case "EndJump":
                EndJump();
                break;

            //#################### Idle ############################
            case "StartIdle":
                StartIdle();
                break;

            //#################### Walk ############################
            case "StartWalk":
                StartWalk();
                break;
            case "EndWalk":
                EndWalk();
                break;

            //#################### Climb ###########################
            case "StartClimb":
                StartClimb();
                break;
            case "EndClimb":
                EndClimb();
                break;

            //ClimbJump
            case "StartClimbJump":
                StartClimbJump();
                break;
            case "ClimbJump":
                ClimbJump();
                break;
            case "EndClimbJump":
                EndClimbJump();
                break;

            //ClimbWall
            case "StartClimbWall":
                StartClimbWall();
                break;
            case "ClimbWall":
                ClimbWall();
                break;
            case "EndClimbWall":
                EndClimbWall();
                break;

            //#################### Dash ############################
            case "StartDash":
                StartDash();
                break;
            case "EndDash":
                EndDash();
                break;

            //#################### Attack ##########################


            //#################### Fall ############################
            case "Falling":
                Falling();
                break;
            case "OnGround":
                //OnGround();
                break;

            default:
                Debug.LogError("Can't found state " + stateName);
                break;
        }
    }

    //####################################################################################

    #region State Callbacks

    #region Jump

    private void StartJump()
    {
        // 產生向上的力道來抵銷玩家角色任何向下的動量
        // 以讓玩家角色向上跳躍
        Jump();
        _flyTime = 0f;
    }
    private void Jump()
    {
        var downVelocity = Mathf.Min(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, jumpSpeed - downVelocity, 0);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
        //_rb.velocity = deltaVelocity;
        _skeletonAnimation.AnimationState.SetAnimation(0, jump, false);
        GameManager.instance.SetControllerVibration(0.3f, 0.3f, 0.1f);
    }
    private void EndJump()
    {
        if (!_dashButton)
        {
            // 產生剛好的力道來抵銷上升的動量
            var upVelocity = Mathf.Max(_rb.velocity.y, 0);
            var deltaVelocity = new Vector3(0, -upVelocity, 0);
            _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
            //_rb.velocity = deltaVelocity;
        }
    }

    #endregion

    #region Idle

    private void StartIdle()
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
    }

    #endregion

    #region Walk

    private void StartWalk()
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, walk, true);
        
    }
    private void EndWalk()
    {
        _skeletonAnimation.timeScale = 1f;
    }

    #endregion

    #region Climb

    private void StartClimb()
    {
        _gravityScale = 0;
        //_rb.AddForce(new Vector2(0, climbSpeed), ForceMode.VelocityChange);
        _rb.velocity = new Vector2(0, climbSpeed);
    }
    private void EndClimb() 
    {
        
    }
    private void StartClimbJump()
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(new Vector3(wallJumpSpeedx * (_isWallOnRight == true ? -1 : 1),0,0), ForceMode.Impulse);
        isWallJumping = true;
        Invoke("SetIsWallJumpingFalse", 0.15f);
        _flyTime = 0f;
        _gravityScale = 1;
    }
    private void SetIsWallJumpingFalse()
    {
        isWallJumping = false;
    }

    private void ClimbJump()
    {
        Jump();
    }
    private void EndClimbJump()
    {
        EndJump();
    }
    private void StartClimbWall()
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(new Vector3(wallJumpSpeedx * (_isWallOnRight == true ? -1 : 1), 0, 0), ForceMode.VelocityChange);
        isWallJumping = true;
        Invoke("ClimbWallCallback", 0.1f);
        _flyTime = 0f;
        _gravityScale = 1;
    }
    private void ClimbWallCallback()
    {
        _rb.AddForce(new Vector3(wallJumpSpeedx * (_isWallOnRight == true ? 1 : -1), 0, 0), ForceMode.Impulse);
        SetIsWallJumpingFalse();
    }
    private void ClimbWall()
    {
        Jump();
    }
    private void EndClimbWall()
    {
        EndJump();
    }

    #endregion

    #region Dash

    private void StartDash()
    {
        _gravityScale = 0f;
        _rb.velocity = Vector3.zero;
        if (!onWall)
            _rb.velocity = new Vector3(dashSpeed * (faceRight ? 1 : -1), 0, 0);
        else
            _rb.velocity = new Vector3(dashSpeed * (!faceRight ? 1 : -1), 0, 0);
        isDashing = true;
        canDash = false;
        nextDashTime = Time.time + dashCooldown;
        Invoke("SetDashingFalse", dashTime);
        GameManager.instance.SetControllerVibration(0.2f, 0.5f, 0.3f);
    }
    private void SetDashingFalse()
    {
        isDashing = false;

    }
    private void EndDash()
    {
        _gravityScale = 1f;
        _rb.velocity = Vector3.zero;
    }

    #endregion

    #region Attack



    #endregion

    #region Fall

    private void Falling()
    {
        _gravityScale = 1f;
    }
    private void OnGround()
    {

    }

    #endregion

    #endregion

    #endregion
}