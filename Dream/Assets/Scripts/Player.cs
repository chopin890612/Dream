using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class Player : MonoBehaviour
{
    [Header("X Axis Movement")]
    [SerializeField] float walkSpeed = 25f;
    [Space(5)]

    [Header("Y Axis Movement")]
    [SerializeField] float jumpSpeed = 45f;
    [SerializeField] float fallSpeed = 45f;
    [SerializeField] int jumpSteps = 20;
    [SerializeField] int jumpThreshold = 7;
    [Space(5)]

    [Header("Ground Setting")]
    [SerializeField] float anchorOffset = 1f;
    [SerializeField] float detectSphereRadius = 0.2f;
    [SerializeField] float detectSphereDistance = 0.5f;
    [SerializeField] bool onGround = false;
    [SerializeField] bool onWall = false;
    [Space(5)]

    [Header("Debugger")]
    [SerializeField] float currentSphereDistance;
    [Space(5)]

    [Header("Aniamtion")]
    public AnimationReferenceAsset walk;
    public AnimationReferenceAsset attack;
    public AnimationReferenceAsset idle;
    public AnimationReferenceAsset jump;


    private bool _attackButton;
    private Vector3 _gravity = new Vector3(0, -50f, 0);
    private InputMaster _inputActions;
    private Animator _animator;
    private Rigidbody _rb;
    private SkeletonAnimation _skeletonAnimation;
    private Spine.EventData _endAttEvent;
    private Spine.EventData _onStepEvenet;

    
    private void Awake()
    {
        _inputActions = new InputMaster();
        _inputActions.Enable();
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }
    private void Update()
    {
        _animator.SetBool("OnGround", onGround);
        _attackButton = _inputActions.Player.Jump.ReadValue<float>() == 1f ? true : false;
        _animator.SetBool("JumpButton", _attackButton);
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_gravity, ForceMode.Acceleration);

        onGround = Physics.SphereCast(transform.position + new Vector3(0, anchorOffset, 0), detectSphereRadius, Vector3.down,
            out RaycastHit hitGround, detectSphereDistance, LayerMask.GetMask("Ground"));


        if (currentSphereDistance < detectSphereDistance)
            currentSphereDistance = hitGround.distance;
        else
            currentSphereDistance = detectSphereDistance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            Debug.Log("Wall");
            onWall = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -currentSphereDistance + anchorOffset, 0), detectSphereRadius);
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
    }
    private void EndJump()
    {
        // 產生剛好的力道來抵銷上升的動量
        var upVelocity = Mathf.Max(_rb.velocity.y, 0);
        var deltaVelocity = new Vector3(0, -upVelocity, 0);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
    }

    #endregion

    #endregion
}