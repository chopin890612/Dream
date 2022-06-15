using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class Player : MonoBehaviour
{
    [Header("X Axis Movement")]
    [SerializeField] float moveSpeed = 25f;
    [SerializeField] bool faceRight = true;
    [Space(5)]

    [Header("Y Axis Movement")]
    [SerializeField] float jumpSpeed = 45f;
    [SerializeField] float climbSpeed = 45f;
    [SerializeField] float fallSpeed = 45f;
    [Space(5)]

    [Header("Ground Settings")]
    [SerializeField] float anchorOffset = 1f;
    [SerializeField] float groundDetectRadius = 0.2f;
    [SerializeField] float groundDetectDistance = 0.5f;
    [SerializeField] bool isWalking = false;
    [SerializeField] bool onGround = false;
    [Space(5)]

    [Header("Wall Settings")]
    [SerializeField] float wallDetectDistance = 1f;
    [SerializeField] Vector3 wallDetectRadius = new Vector3(1,1,1);
    [SerializeField] bool onWall = false;
    [SerializeField] bool isWallJumping = false;
    [SerializeField] float wallJumpSpeedx;
    [SerializeField] float wallJumpSpeedy;
    [Space(5)]

    [Header("Debugger")]
    [SerializeField] float currentSphereDistance;
    [SerializeField] float currentBoxDistance;
    [Space(5)]

    [Header("Aniamtion")]
    [SerializeField] AnimationReferenceAsset walk;
    [SerializeField] AnimationReferenceAsset attack;
    [SerializeField] AnimationReferenceAsset idle;
    [SerializeField] AnimationReferenceAsset jump;


    private bool _jumpButton;
    private Vector3 _gravity = new Vector3(0, -50f, 0);
    public float _gravityScale = 1f;
    private InputMaster _inputActions;
    private Animator _animator;
    private Rigidbody _rb;
    private SkeletonAnimation _skeletonAnimation;
    private Spine.EventData _endAttEvent;
    private Spine.EventData _onStepEvenet;
    private float _flyTime = 0f;
    private float _playerDirection;
    private GameObject _spineRenderer;
    private bool _isWallOnRight =true;

    
    private void Awake()
    {
        _inputActions = new InputMaster();
        _inputActions.Enable();
    }
    private void Start()
    {
        _spineRenderer = transform.GetChild(0).gameObject;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _skeletonAnimation = _spineRenderer.GetComponent<SkeletonAnimation>();
    }
    private void Update()
    {
        _animator.SetBool("OnGround", onGround);
        _jumpButton = _inputActions.Player.Jump.ReadValue<float>() == 1f ? true : false;
        _animator.SetBool("JumpButton", _jumpButton);
        
        _animator.SetFloat("FlyTime", _flyTime);
        _playerDirection = _inputActions.Player.Movment.ReadValue<float>();

        isWalking = _playerDirection != 0f;
        _animator.SetBool("IsWalking", isWalking);

        _animator.SetBool("OnWall", onWall);

        _animator.SetBool("IsWallJumping", isWallJumping);

        _animator.SetBool("IsMoveToWall", _isWallOnRight == faceRight);
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_gravity * _gravityScale, ForceMode.Acceleration);
        if (_rb.velocity.y < fallSpeed)
            _rb.velocity = new Vector2(_rb.velocity.x, fallSpeed);

        onGround = Physics.SphereCast(transform.position + new Vector3(0, anchorOffset, 0), groundDetectRadius, Vector3.down,
            out RaycastHit hitGround, groundDetectDistance, LayerMask.GetMask("Ground"));

        onWall = Physics.BoxCast(transform.position + new Vector3(0, anchorOffset, 0), wallDetectRadius / 2, faceRight? Vector3.right : Vector3.left,
                 out RaycastHit hitwall, Quaternion.identity, wallDetectDistance, LayerMask.GetMask("Wall"));
        if (onWall)
            _isWallOnRight = faceRight;

        if(!isWallJumping)
            _rb.velocity = new Vector3(_playerDirection * moveSpeed, _rb.velocity.y);
        if (isWalking)
            _skeletonAnimation.timeScale = Mathf.Abs(_playerDirection);

        if (onGround == false)
            _flyTime += Time.deltaTime;

        //if (isUpdateClimbJump)
        //{
        //    var downVelocity = Mathf.Min(_rb.velocity.y, 0);
        //    var deltaVelocity = new Vector3(0, jumpSpeed - downVelocity, 0);
        //    _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
        //}



        PlayerFlip();


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
        Gizmos.DrawWireCube(transform.position + new Vector3(faceRight? currentBoxDistance : -currentBoxDistance, anchorOffset), wallDetectRadius);
    }
    public void StateCallback(string stateName)
    {
        switch (stateName)
        {
            case "StartJump":
                StartJump();
                break;
            case "EndJump":
                EndJump();
                break;
            case "StartIdle":
                StartIdle();
                break;
            case "StartWalk":
                StartWalk();
                break;
            case "EndWalk":
                EndWalk();
                break;
            case "StartClimb":
                StartClimb();
                break;
            case "EndClimb":
                EndClimb();
                break;
            case "StartClimbJump":
                StartClimbJump();
                break;
            case "EndClimbJump":
                EndClimbJump();
                break;
            case "KeepClimbJumpPhase":
                KeepClimbJumpPhase();
                break;
            case "EndKCJPhase":
                EndKCJPhase();
                break;
            case "Falling":
                Falling();
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
        var downVelocity = Mathf.Min(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, jumpSpeed - downVelocity, 0);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
        _skeletonAnimation.AnimationState.SetAnimation(0, jump, false);

        _flyTime = 0f;
    }
    private void EndJump()
    {
        // 產生剛好的力道來抵銷上升的動量
        var upVelocity = Mathf.Max(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, -upVelocity, 0);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
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
        _rb.velocity = new Vector2(0, climbSpeed);
    }
    private void EndClimb() 
    {
        
    }
    private void StartClimbJump()
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(new Vector3(wallJumpSpeedx * (faceRight == true ? -1 : 1),wallJumpSpeedy,0), ForceMode.Impulse);
        isWallJumping = true;
        Invoke("SetIsWallJumpingFalse", 0.1f);
        _flyTime = 0f;
        _gravityScale = 1;
    }
    private void EndClimbJump()
    {
        _gravityScale = 1f;
    }
    private void SetIsWallJumpingFalse()
    {
        isWallJumping = false;
    }
    private void KeepClimbJumpPhase()
    {

    }
    private void EndKCJPhase()
    {

    }

    #endregion

    private void Falling()
    {
        _gravityScale = 1f;
        //_rb.AddForce(new Vector2(0, fallSpeed), ForceMode.VelocityChange);
    }

    #endregion

    private void PlayerFlip()
    {
        if(_playerDirection > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            faceRight = true;
        }
        else if(_playerDirection < 0)
        {
            transform.localScale = Vector3.one;
            faceRight = false;
        }
    }
}