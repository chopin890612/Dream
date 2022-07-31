using System.Collections;
using UnityEngine;
using Bang.StateMachine.BossState;
using Bang.StateMachine;

public class Boss : MonoBehaviour
{
    #region State Variables
    public StateMachine<Boss, EnemyData> stateMachine;
    public IdleState idleState;
    public RunState runState;
    public TailAttack tailAttack;
    public DashAttack dashAttack;

    #endregion

    #region State Parameters
    public bool CanSlope;
    public bool seePlayer;
    public bool playerInAttackRange;
    public bool playerInFarRange;
    public bool playerBeside;
    public float attackTime;
    public float abilityCooldown;
    public float knockBackTime;
    public bool onGround;
    public bool onWall;

    [SerializeField] private EnemyData enemyData;
    #endregion

    #region Components
    private Rigidbody _rb;
    public Animator animator;
    public CombatController combatController;
    #endregion

    #region Combat
    public Collider touchedCollider;
    #endregion

    #region Check Parameters
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckSize;

    [SerializeField] private Transform _wallCheckPoint;
    [SerializeField] private Vector3 _wallCheckSize;

    [SerializeField] private Transform _seeCheckPoint;
    [SerializeField] private float _seeCheckSize;
    [SerializeField] private GameObject _seePlayerObject;

    [SerializeField] private Transform _besideCheckPoint;
    [SerializeField] private float _besideCheckSize;

    [SerializeField] private Transform _attackCheckPoint;
    [SerializeField] private float _attackCheckSize;

    [SerializeField] private Transform _attackCheckPoint2;
    [SerializeField] private float _attackCheckSize2;

    [SerializeField] private float _platformSlopeCheckLength;
    #endregion

    #region Layers & Tags
    [SerializeField] private LayerMask _playeLayer;
    [SerializeField] private LayerMask _groundLayer;

    #endregion

    [SerializeField] private int faceDir = 1;
    [SerializeField] string currentState;
    private Vector2 _platformNormal;

    #region Unity Lifecycles
    private void Awake()
    {
        stateMachine = new StateMachine<Boss, EnemyData>();
        idleState = new IdleState(this, stateMachine, enemyData);
        runState = new RunState(this, stateMachine, enemyData);
        tailAttack = new TailAttack(this, stateMachine, enemyData);
        dashAttack = new DashAttack(this, stateMachine, enemyData);
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        stateMachine.Initalize(idleState);
    }
    private void Update()
    {
        stateMachine.currentState.LogicUpdate();

        #region Checks
        attackTime -= Time.deltaTime;
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
        playerBeside = Physics.CheckSphere(_besideCheckPoint.position, _besideCheckSize, _playeLayer);

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_besideCheckPoint.position, _besideCheckSize);

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
    #region Move Methods
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
    public void ChangeFace()
    {
        int playerSide = faceDir;
        if (seePlayer)
            playerSide = (int)Mathf.Sign((_seePlayerObject.transform.position - transform.position).x);
        if (playerBeside == false && playerSide != faceDir)
            Turn();
    }
    #endregion

    #region Combat Methods
    public void DashAttack()
    {
        _rb.velocity = Vector3.zero;
        attackTime = enemyData.attackSpeed;
        abilityCooldown = enemyData.abilityCooldown;
        GameManager.instance.DoForSeconds(() => _rb.AddForce(new Vector2(30 * faceDir, 0), ForceMode.Impulse), (25f / 60f) / 0.3f);
    }
    public void TailAttack()
    {
        _rb.velocity = Vector3.zero;
        attackTime = enemyData.attackSpeed;
    }
    public void EndAttack()
    {
        if (stateMachine.currentState == tailAttack)
            tailAttack.EndAttack();
        else if (stateMachine.currentState == dashAttack)
            dashAttack.EndAttack();
    }
    #endregion
}