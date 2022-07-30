using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;
using Bang.StateMachine.EnemyMachine;
using Spine.Unity;

public class Enemy : MonoBehaviour
{
    #region State Varibles
    public StateMachine<Enemy, EnemyData> stateMachine;
    public IdleState idleState;
    public RunState runState;
    public ChaseState chaseState;
    public AttackState attackState;
    public KnockBackState knockBackState;
    public DashAttack dashAttackState;

    [SerializeField] private EnemyData enemyData;
    #endregion

    #region State Parameters
    public bool CanSlope;
    public bool seePlayer;
    public bool playerInAttackRange;
    public bool playerInFarRange;
    public float attackTime;
    public float abilityCooldown;
    public float knockBackTime;
    #endregion

    #region Component
    private Rigidbody _rb;
    public GameObject attackCollision;
    public CombatController combatSystem;
    public Animator animator;
    #endregion

    #region Animations
    #endregion

    #region Combat
    public Collider touchedCollision;
    #endregion

    #region CHECKS PARAMETERS
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckSize;
    [SerializeField] public bool onGround;

    [SerializeField] private Transform _wallCheckPoint;
    [SerializeField] private Vector3 _wallCheckSize;
    [SerializeField] public bool onWall;

    [SerializeField] private Transform _seeCheckPoint;
    [SerializeField] private float _seeCheckSize;
    [SerializeField] private GameObject _seePlayerObject;

    [SerializeField] private Transform _attackCheckPoint;
    [SerializeField] private float _attackCheckSize;

    [SerializeField] private Transform _attackCheckPoint2;
    [SerializeField] private float _attackCheckSize2;

    [SerializeField] private float _platformSlopeCheckLength;
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _playeLayer;
    #endregion

    [SerializeField] private int faceDir = 1;
    private Vector2 _platformNormal;
    [SerializeField] private string currentState;
    private float _seeDistance;

    #region Unity LifeCycles
    private void Awake()
    {
        stateMachine = new StateMachine<Enemy, EnemyData>();
        idleState = new IdleState(this, stateMachine, enemyData);
        runState = new RunState(this, stateMachine, enemyData);
        chaseState = new ChaseState(this, stateMachine, enemyData);
        attackState = new AttackState(this, stateMachine, enemyData);
        knockBackState = new KnockBackState(this, stateMachine, enemyData);
        dashAttackState = new DashAttack(this, stateMachine, enemyData);
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        stateMachine.Initalize(runState);
        attackCollision.SetActive(false);
        //combatSystem.hurtEvent.AddListener(Hurt);
        combatSystem.superArmorBreakEvent.AddListener(Hurt);
    }
    private void Update()
    {
        stateMachine.currentState.LogicUpdate();

        #region CHECKS
        attackTime -= Time.deltaTime;
        knockBackTime -= Time.deltaTime;
        abilityCooldown -= Time.deltaTime;

        //Chase Check
        var objects = Physics.OverlapSphere(_seeCheckPoint.position, _seeCheckSize, _playeLayer);
        if (objects.Length > 0)
        {
            _seePlayerObject = objects[0].transform.gameObject;
            seePlayer = true;
        }
        else
        {
            _seePlayerObject = null;
            seePlayer = false;
        }

        //AttackRange Check
        playerInAttackRange = Physics.CheckSphere(_attackCheckPoint.position, _attackCheckSize, _playeLayer);
        playerInFarRange = Physics.CheckSphere(_attackCheckPoint2.position, _attackCheckSize2, _playeLayer);

        //Ground Check
        onGround = Physics.CheckSphere(_groundCheckPoint.position, _groundCheckSize, _groundLayer);

        //Wall Check
        onWall = Physics.CheckBox(_wallCheckPoint.position, _wallCheckSize, Quaternion.identity, _groundLayer);

        //Platform Slope Check
        Physics.Raycast(transform.position, Vector2.down, out RaycastHit hit, _platformSlopeCheckLength, _groundLayer);
        _platformNormal = hit.normal;
        #endregion

        if((onGround == false || onWall == true || CanSlope == false) && stateMachine.currentState == runState)
        {
            Turn();
        }

        CanSlope = Mathf.Abs(Vector2.Angle(_platformNormal, Vector2.up)) < enemyData.maxSlopeAngle;

        currentState = stateMachine.currentState.ToString();
    }
    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_wallCheckPoint.position, _wallCheckSize * 2);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_seeCheckPoint.position, _seeCheckSize);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_attackCheckPoint.position, _attackCheckSize);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_attackCheckPoint2.position, _attackCheckSize2);
    }

    private void Turn()
    {
        //Vector3 scale = transform.localScale; //stores scale and flips x axis, "flipping" the entire gameObject around. (could rotate the player instead)
        //scale.x *= -1;
        //transform.localScale = scale;
        transform.Rotate(0, 180, 0);

        faceDir *= -1;
    }

    #region Movment Methods
    public void Drag(float amount)
    {
        Vector2 force = amount * _rb.velocity.normalized;
        force.x = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(force.x)); //ensures we only slow the player down, if the player is going really slowly we just apply a force stopping them
        force.y = Mathf.Min(Mathf.Abs(_rb.velocity.y), Mathf.Abs(force.y));
        force.x *= Mathf.Sign(_rb.velocity.x); //finds direction to apply force
        force.y *= Mathf.Sign(_rb.velocity.y);

        _rb.AddForce(-force, ForceMode.Impulse); //applies force against movement direction
    }
    public void Run(float lerpAmount, bool walkSlope, float multiple)
    {
        float targetSpeed = enemyData.runMaxSpeed * faceDir * multiple;
        float speedDif = targetSpeed - _rb.velocity.x; //calculate difference between current velocity and desired velocity

        #region Acceleration Rate
        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? enemyData.runAccel : enemyData.runDeccel;
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = enemyData.stopPower;
        }
        else if (Mathf.Abs(_rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(_rb.velocity.x)))
        {
            velPower = enemyData.turnPower;
        }
        else
        {
            velPower = enemyData.accelPower;
        }
        #endregion

        //applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(_rb.velocity.x, movement, lerpAmount);

        Vector2 moveDir = Vector2.right;
        if (walkSlope && CanSlope)
        {
            moveDir = Vector2.Perpendicular(_platformNormal).normalized * -1;
            Debug.DrawRay(transform.position, moveDir, Color.red);
        }
        _rb.AddForce(movement * moveDir); //applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 
    }
    public void Chase(float chaseSpeed)
    {
        if (_seePlayerObject == null)
            return;
        var playerSide = (int)Mathf.Sign((_seePlayerObject.transform.position - transform.position).x);
        if (playerSide != faceDir)
            Turn();
        Run(1, true, chaseSpeed);
    }
    #endregion

    #region CombatMethods
    public void Attack()
    {
        //StartCoroutine(AttackDelay());
        attackTime = enemyData.attackSpeed;
    }
    public void EndAttack()
    {
        if(stateMachine.currentState == attackState)
            attackState.EndAttack();
        else if(stateMachine.currentState == dashAttackState)
            dashAttackState.EndAttack();
    }
    public void DashAttack()
    {
        abilityCooldown = enemyData.abilityCooldown;
        attackTime = enemyData.attackSpeed;
        GameManager.instance.DoForSeconds(() => _rb.AddForce(new Vector2(20, 0) * faceDir, ForceMode.Impulse), 0.25f) ;
    }
    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(1f);
        attackCollision.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackCollision.SetActive(false);
        attackTime = enemyData.attackSpeed;
        attackState.EndAttack();
    }
    public void KnockBack(float forceScale)
    {
        _rb.velocity = Vector3.zero;
        Vector3 force = new Vector3(enemyData.knockBackForce.x, enemyData.knockBackForce.y);
        force.x *= -1f * Mathf.Sign(touchedCollision.GetComponent<AttackCollision>().owner.transform.position.x - transform.position.x);
        _rb.AddForce(force * forceScale, ForceMode.Impulse);
    }
    public void Hurt(Collider collider)
    {
        touchedCollision = collider;
        knockBackTime = enemyData.knockBackTime;
    }
    #endregion
}
