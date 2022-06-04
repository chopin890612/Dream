using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private bool onGround;
    [SerializeField]
    private bool touchWall;

    [Header("Jump")]
    [SerializeField]
    private float jumpRadius = 0.1f;
    [SerializeField]
    private float jumpDistance = 0.5f;
    [SerializeField]
    private float jumpVelocity = 1f;
    [Header("Move")]
    [SerializeField]
    private float moveRadius = 0.1f;
    [SerializeField]
    private float moveDistance = 0.5f;
    [SerializeField]
    private float moveVelocity = 1f;

    [Header("Debug")]
    [SerializeField]
    private float currentDistance;
    [SerializeField]
    private float attackComboTime;
    [SerializeField]
    private bool isComboTime;


    private InputMaster inputActions;
    private Animator animator;
    private bool startAnimation ,endAnimation;
    private bool startJump, stopJump, startAttack, endAttack, firstAttack, stopAttack;
    private bool isAnimation, isAttack;
    private Rigidbody rb;
    private float flyTime = 0;
    private float playerFaceDir;
    private int attackPhase = 0;
    private void Awake()
    {
        inputActions = new InputMaster();
        inputActions.Enable();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        animator.SetBool("OnGround", onGround);
        animator.SetBool("JumpButton", inputActions.Player.Jump.ReadValue<float>() == 1 ? true : false);
        if(!startAnimation)
            animator.SetBool("AttackButton", inputActions.Player.Attack.ReadValue<float>() == 1 ? true : false);
        animator.SetBool("EndAttack", endAttack);
        //animator.SetBool("StartAnimation", startAnimation);
        //animator.SetBool("EndAnimation", endAnimation);

        if (!onGround) flyTime += Time.deltaTime;
        animator.SetFloat("FlyTime", flyTime);
        playerFaceDir = inputActions.Player.Movment.ReadValue<float>();
        
        if (isComboTime)
            attackComboTime += Time.deltaTime;
        animator.SetFloat("ComboTime", attackComboTime);

        #region Attack States
        if (firstAttack)
        {
            attackPhase++;
            //Debug.Log("Attack" + attackPhase);
            firstAttack = false;
        }
        if (startAttack)
        {
            attackPhase++;
            //Debug.Log("Attack" + attackPhase);
            isComboTime = false;
            startAnimation = true;
            startAttack = false;
        }
        if (endAttack)
        {            
            isComboTime = true;
            attackComboTime = 0f;
            endAttack = false;
        }
        if (stopAttack)
        {
            attackComboTime = 0f;
            attackPhase = 0;
            isComboTime = false;
            stopAttack = false;
        }
        if (startAnimation)
        {
            animator.SetTrigger("StartAnimation");
            startAnimation = false;
        }
        if (endAnimation)
        {
            animator.SetTrigger("EndAnimation");            
            endAttack = true;
            endAnimation = false;
        }
        #endregion
    }

    void FixedUpdate()
    {
        #region Move States
        onGround = Physics.SphereCast(transform.position, jumpRadius, Vector3.down, 
            out RaycastHit hitGround, jumpDistance, LayerMask.GetMask("Ground"));
        if (startJump)
        {
            // ���ͦV�W���O�D�ө�P���a�������V�U���ʶq
            // �H�����a����V�W���D�A���ӤG�q���ݭn��
            var downVelocity = Mathf.Min(rb.velocity.y, 0);
            var deltaVelocity = new Vector3(0, jumpVelocity - downVelocity, 0);
            rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
            startJump = false;
        }
        if (stopJump)
        {
            // ���ͭ�n���O�D�ө�P�W�ɪ��ʶq
            var upVelocity = Mathf.Max(rb.velocity.y, 0);
            var deltaVelocity = new Vector3(0, -upVelocity, 0);
            rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
            stopJump = false;
        }

        //touchWall = Physics.SphereCast(transform.position, moveRadius, new Vector3(playerFaceDir, 0, 0).normalized,
        //    out RaycastHit hitWall, faceDistance, LayerMask.GetMask("Wall"));
        touchWall = Physics.BoxCast(transform.position, new Vector3(moveRadius/2, 0.95f/2, 1), new Vector3(playerFaceDir, 0, 0).normalized,
            out RaycastHit hitWall, Quaternion.identity, moveDistance, LayerMask.GetMask("Wall"));
        if (touchWall)
        {
            var moveVelocity = rb.velocity.x;
            rb.AddForce(new Vector3(-moveVelocity, 0, 0), ForceMode.VelocityChange);
        }
        else
        {
            ChangeFace();
            var moveSpeed = playerFaceDir * moveVelocity;
            rb.velocity = new Vector3(moveSpeed, rb.velocity.y, 0);
        }
        #endregion

        

        if (onGround)
            currentDistance = hitGround.distance;
        else
            currentDistance = jumpDistance;

    }
    private void OnDrawGizmosSelected()
    {
        // Show "Jump" SphereCast
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * currentDistance, jumpRadius);

        // Show "Move" SphereCast
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(transform.position + new Vector3(playerFaceDir, 0, 0).normalized * moveDistance, moveRadius);

        // Show "Move" BoxCast
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + new Vector3(playerFaceDir, 0, 0).normalized * moveDistance, new Vector3(moveRadius, 0.95f,1));
    }
    private void ChangeFace()
    {
        if (endAnimation)
        {
            if (playerFaceDir > 0)
            {
                transform.localScale = Vector3.one;
            }
            if (playerFaceDir < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
    public void FunctionHandler(string functionName)
    {
        switch (functionName)
        {
            case "StartJump" :
                startJump = true;
                break;
            case "StopJump":
                stopJump = true;
                break;
            case "OnGround":
                flyTime = 0;
                break;
            case "FirstAttack":
                firstAttack = true;
                break;
            case "StartAttack":
                startAttack = true;
                break;
            case "EndAttack" :
                endAttack = true;
                break;
            case "StopAttack":
                stopAttack = true;
                break;
            case "StartAnimation":
                startAnimation = true;
                break;
            case "EndAniamtion":
                endAnimation = true;
                break;
        }
    }
}