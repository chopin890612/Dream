using System.Collections;
using UnityEngine;
using Bang.StateMachine;
using Bang.StateMachine.PlayerMachine;
using Spine.Unity;
using UnityEngine.Events;


public class TestPlayer : MonoBehaviour
{
    #region State Variables
    public StateMachine<TestPlayer, PlayerData> stateMachine { get; private set; }
    public IdleState idleState { get; private set; }
    public RunState runState { get; private set; }
    public JumpState jumpState { get; private set; }
    public OnAirState onAirState { get; private set; }
    public DashState dashState { get; private set; }
    public WallIdleState wallIdleState { get; private set; }
    public WallSlideState wallSlideState { get; private set; }
    public WallJumpState wallJumpState { get; private set; }
    public AttackState attackState { get; private set; }
    public KnockBackState knockBackState { get; private set; }

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Game Datas
    public SaveStatus statues;
    #endregion

    #region Components
    public Rigidbody _rb { get; private set; }
    public CombatController combatSystem;
    public float gravityScale;
    public PhysicMaterial noFriction;
    public PhysicMaterial infFrction;
    public GameObject Fire;
    #endregion

    #region Animations
    [Header("Spine Animations")]
    public GameObject spineRenderer;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walk, jump, fall, attack;
    public EventDataReferenceAsset startCollision, endCollision, endAttack;
    [Space(5)]
    [Header("Animator")]
    public GameObject spriteRenderer;
    public Animator animator;
    #endregion

    #region Combat
    [Header("Combat")]
    public GameObject attackCollision;
    public GameObject spriteAttackCollision;
    public Collider bodyCollider;
    public Collider touchedCollision;

    public int attackCount = 0;
    #endregion

    #region STATE PARAMETERS
    public bool IsFacingRight { get; private set; }
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }
    public float LastAttackTime { get; private set; }
    public float AttackCooldown { get; private set; }
    public float LastKnockBackTime { get; private set; }
    public float AttackResetTime { get; private set; }
    public float AtEdgeTime { get; private set; }
    public bool CanSlope { get; private set; }
    public bool IsPassingPlatform { get; private set; }
    public bool IsInChangeWorld { get; private set; }
    public bool IsSetFire { get; private set; }
    #endregion

    #region INPUT PARAMETERS
    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    public bool isPressJump;
    public bool isPressChangeWorld;
    #endregion

    #region CHECK PARAMETERS
    [Header("Checks")]
    [SerializeField] private Transform _headCheckPoint;
    [SerializeField] private float _headCheckSize;
    [Space(5)]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckSize;
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize;
    [Space(5)]
    [SerializeField] private Transform _platformSlopeCheck;
    [SerializeField] private float _platformSlopeCheckSize;
    [SerializeField] private float _platformSlopeCheckLength;
    [Space(5)]
    [SerializeField] private Transform _edgeClimbCheck;
    [SerializeField] private float _edgeClimbCheckSize;
    [Space(5)]
    [SerializeField] private CapsuleCollider _changeWorldCheck;
    [SerializeField] private float _minRadius = 2;
    [SerializeField] private float _maxRadius = 16;
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _player;
    [SerializeField] private LayerMask _walkLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _canPassPlatform;
    #endregion

    #region Debugger
    [Header("Debugger")]
    [SerializeField] string CurrentState;
    [SerializeField] Vector3 _platformNormal;
    private float groundDistance;
    private float wallDistance;
    [SerializeField] private Vector2 inputM;
    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        stateMachine = new StateMachine<TestPlayer, PlayerData>();

        idleState = new IdleState(this, stateMachine,playerData);
        runState = new RunState(this, stateMachine, playerData);
        jumpState = new JumpState(this, stateMachine, playerData);
        onAirState = new OnAirState(this, stateMachine, playerData);
        dashState = new DashState(this, stateMachine, playerData);
        wallIdleState = new WallIdleState(this, stateMachine, playerData);
        wallSlideState = new WallSlideState(this, stateMachine, playerData);
        wallJumpState = new WallJumpState(this, stateMachine, playerData);
        attackState = new AttackState(this, stateMachine, playerData);
        knockBackState = new KnockBackState(this, stateMachine, playerData);

        _rb = GetComponent<Rigidbody>();
        skeletonAnimation = spineRenderer.GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.Event += AttackAnimationHandler;
        animator = spriteRenderer.GetComponent<Animator>();

        combatSystem.hurtEvent.AddListener(Hurt);
    }
    private void Start()
    {
        InputHandler.instance.OnJumpPressed += args => OnJump(args);
        InputHandler.instance.OnJumpReleased += args => OnJumpUp(args);
        InputHandler.instance.OnDash += args => OnDash(args);
        InputHandler.instance.OnAttack += args => OnAttack(args);
        InputHandler.instance.OnChangeWorld += args => OnChangeWorld(args);
        InputHandler.instance.ReleaseChangeWorld += args => ReleaseChangeWorld(args);
        InputHandler.instance.OnFire += arg => OnFire(arg);

        SetGravityScale(playerData.gravityScale);

        stateMachine.Initalize(idleState);
        IsFacingRight = true;

        attackCollision.SetActive(false);
    }
    private void Update()
    {
        stateMachine.currentState.LogicUpdate();
        inputM = InputHandler.instance.Movement;

        #region CHECKS
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        LastAttackTime -= Time.deltaTime;
        LastKnockBackTime -= Time.deltaTime;
        AtEdgeTime -= Time.deltaTime;

        AttackCooldown -= Time.deltaTime;
        AttackResetTime -= Time.deltaTime;
        if (AttackResetTime < 0)
            ResetAttackCount();
        

        //Head Check
        if(Physics.CheckSphere(_headCheckPoint.position, _headCheckSize, _canPassPlatform))
        {
            PassPlatform(0.2f);
        }

        //Ground Check
        if (IsPassingPlatform)
        {
            if (Physics.CheckSphere(_groundCheckPoint.position, _groundCheckSize, _groundLayer))//checks if set box overlaps with ground
            {
                LastOnGroundTime = playerData.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }
        }
        else
        {
            if (Physics.CheckSphere(_groundCheckPoint.position, _groundCheckSize, _walkLayer)) //checks if set box overlaps with ground
            {
                LastOnGroundTime = playerData.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }
        }

        if (IsPassingPlatform)
        {
            //Right Wall Check
            if ((Physics.CheckBox(_frontWallCheckPoint.position, _wallCheckSize, Quaternion.identity, _groundLayer) && IsFacingRight))
                LastOnWallRightTime = playerData.coyoteTime;

            //Left Wall Check
            if (Physics.CheckBox(_backWallCheckPoint.position, _wallCheckSize, Quaternion.identity, _groundLayer) && !IsFacingRight)
                LastOnWallLeftTime = playerData.coyoteTime;
        }
        else
        {
            //Right Wall Check
            if ((Physics.CheckBox(_frontWallCheckPoint.position, _wallCheckSize, Quaternion.identity, _walkLayer) && IsFacingRight))
                LastOnWallRightTime = playerData.coyoteTime;

            //Left Wall Check
            if (Physics.CheckBox(_backWallCheckPoint.position, _wallCheckSize, Quaternion.identity, _walkLayer) && !IsFacingRight)
                LastOnWallLeftTime = playerData.coyoteTime;
        }

        //EdgeClimb Check
        if(!Physics.CheckSphere(_edgeClimbCheck.position, _edgeClimbCheckSize, _wallLayer))
            AtEdgeTime = playerData.coyoteTime;
        
        LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        //Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides

        //Platform Slope Check
        Physics.SphereCast(_platformSlopeCheck.position, _platformSlopeCheckSize, Vector3.down, out RaycastHit hitVertical, _platformSlopeCheckLength, _walkLayer);
        _platformNormal = hitVertical.normal;
        
        groundDistance = hitVertical.distance;
        CanSlope = Mathf.Abs(Vector2.Angle(_platformNormal, Vector2.up)) < playerData.maxSlopeAngle;

        //ChangWorld Range Check
        //Physics.SphereCastAll(transform.position, )
        #endregion

        CurrentState = stateMachine.currentState.ToString();
    }
    private void FixedUpdate()
    {
        UseGravity();
        stateMachine.currentState.PhysicsUpdate();        
    }

    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize * 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize * 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_headCheckPoint.position, _headCheckSize);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_edgeClimbCheck.position, _edgeClimbCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_platformSlopeCheck.position + new Vector3(0, -groundDistance), _platformSlopeCheckSize);
    }

    private void PassPlatform(float time)
    {
        IsPassingPlatform = true;
        Physics.IgnoreLayerCollision(3, 8, true);
        Invoke("DisableCollision", time);
    }
    private void DisableCollision()
    {
        Physics.IgnoreLayerCollision(3, 8, false);
        IsPassingPlatform = false;
    }

    private void Turn()
    {
        //Vector3 scale = transform.localScale; //stores scale and flips x axis, "flipping" the entire gameObject around. (could rotate the player instead)
        //scale.x *= -1;
        //transform.localScale = scale;

        spineRenderer.transform.Rotate(0, 180, 0);
        spriteRenderer.transform.Rotate(0, 180, 0);

        IsFacingRight = !IsFacingRight;
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }
    private void UseGravity()
    {
        //Change globle gravity
        //Physics.gravity = playerData.gravity * gravityScale;

        //Custom Gravity
        if (LastOnGroundTime > 0 && CanSlope)
        {
            Vector2 gravityWay = _platformNormal * -1;
            _rb.AddForce(9.81f * gravityScale * gravityWay);
        }
        else
        {
            _rb.AddForce(playerData.gravity * gravityScale);
        }
    }
    public void SetGravityScale(float scaleValue)
    {
        gravityScale = scaleValue;
    }
    public void ResetLastOnGroundTime()
    {
        LastOnGroundTime = 0;
    }
    #region INPUT CALLBACKS
    //These functions are called when an even is triggered in my InputHandler. You could call these methods through a if(Input.GetKeyDown) in Update
    public void OnJump(InputHandler.InputArgs args)
    {
        isPressJump = true;
        if (InputHandler.instance.Movement.y < -0.5f)
        {
            PassPlatform(0.5f);
        }
        else
            LastPressedJumpTime = playerData.jumpBufferTime;
    }

    public void OnJumpUp(InputHandler.InputArgs args)
    {
        isPressJump = false;
        if (jumpState.CanJumpCut())
            JumpCut();
    }

    public void OnDash(InputHandler.InputArgs args)
    {
        LastPressedDashTime = playerData.dashBufferTime;
    }
    public void OnAttack(InputHandler.InputArgs args)
    {
        LastAttackTime = playerData.attackBufferTime;
        AttackResetTime = playerData.attackResetTime;
    }
    public void OnChangeWorld(InputHandler.InputArgs args)
    {
        if (IsInChangeWorld == false && LastOnGroundTime > 0 && statues.Have_Relic_Lily)
        {
            IsInChangeWorld = true;
            isPressChangeWorld = true;
            ChangeWorld();
        }
    }
    public void ReleaseChangeWorld(InputHandler.InputArgs args)
    {
        isPressChangeWorld = false;        
    }
    public void OnFire(InputHandler.InputArgs args)
    {
        if(statues.Have_Relic_Fire)
            SetFire();
    }
    #endregion

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
    public void Run(float lerpAmount, bool needSlope)
    {
        float targetSpeed = InputHandler.instance.Movement.x * playerData.runMaxSpeed; //calculate the direction we want to move in and our desired velocity
        float speedDif = targetSpeed - _rb.velocity.x; //calculate difference between current velocity and desired velocity

        #region Acceleration Rate
        float accelRate;

        //gets an acceleration value based on if we are accelerating (includes turning) or trying to stop (decelerating). As well as applying a multiplier if we're air borne
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccel : playerData.runDeccel;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccel * playerData.accelInAir : playerData.runDeccel * playerData.deccelInAir;

        //if we want to run but are already going faster than max run speed
        if (((_rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (_rb.velocity.x < targetSpeed && targetSpeed < -0.01f)) && playerData.doKeepRunMomentum)
        {
            accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
        }
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = playerData.stopPower;
        }
        else if (Mathf.Abs(_rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(_rb.velocity.x)))
        {
            velPower = playerData.turnPower;
        }
        else
        {
            velPower = playerData.accelPower;
        }
        #endregion

        //applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(_rb.velocity.x, movement, lerpAmount);

        if (isPressChangeWorld)
            movement *= playerData.pressChangeWorldRunSpeed;
 
        Vector2 moveDir = Vector2.right;
        if (needSlope && CanSlope)
        {            
            moveDir = Vector2.Perpendicular(_platformNormal).normalized * -1;
            //Debug.DrawRay(transform.position, moveDir, Color.yellow);
        }

        _rb.AddForce(movement * moveDir, ForceMode.Force); //applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 
        //_rb.velocity = movement * moveDir;

        if (InputHandler.instance.Movement.x != 0)
            CheckDirectionToFace(InputHandler.instance.Movement.x > 0);
    }
    public void SlopeRun(float lerpAmount)
    {
        float targetSpeed = InputHandler.instance.Movement.x * playerData.runMaxSpeed; //calculate the direction we want to move in and our desired velocity
        float speedDif = targetSpeed - _rb.velocity.x; //calculate difference between current velocity and desired velocity

        #region Acceleration Rate
        float accelRate;

        //gets an acceleration value based on if we are accelerating (includes turning) or trying to stop (decelerating). As well as applying a multiplier if we're air borne
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccel : playerData.runDeccel;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccel * playerData.accelInAir : playerData.runDeccel * playerData.deccelInAir;

        //if we want to run but are already going faster than max run speed
        if (((_rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (_rb.velocity.x < targetSpeed && targetSpeed < -0.01f)) && playerData.doKeepRunMomentum)
        {
            accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
        }
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = playerData.stopPower;
        }
        else if (Mathf.Abs(_rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(_rb.velocity.x)))
        {
            velPower = playerData.turnPower;
        }
        else
        {
            velPower = playerData.accelPower;
        }
        #endregion

        //applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(_rb.velocity.x, movement, lerpAmount);
        Debug.Log(movement);

        Vector2 moveDir = Vector2.right;
        if (CanSlope)
        {
            moveDir = Vector2.Perpendicular(_platformNormal).normalized * -1;
            //Debug.DrawRay(transform.position, moveDir, Color.yellow);
        }

        _rb.velocity = moveDir * Mathf.Sign(movement) * 10;

        if (InputHandler.instance.Movement.x != 0)
            CheckDirectionToFace(InputHandler.instance.Movement.x > 0);
    }
    public void Jump()
    {
        //ensures we can't call a jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        _rb.velocity = Vector2.zero;
        #region Perform Jump
        float adjustedJumpForce = playerData.jumpForce;
        if (_rb.velocity.y < 0)
            adjustedJumpForce -= _rb.velocity.y;

        _rb.AddForce(Vector2.up * adjustedJumpForce, ForceMode.Impulse);
        #endregion
    }
    private void JumpCut()
    {
        if(stateMachine.currentState != jumpState)
        {
            //Debug.Log("JumpCut in " + stateMachine.currentState.ToString());
        }
        //applies force downward when the jump button is released. Allowing the player to control jump height
        _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - playerData.jumpCutMultiplier), ForceMode.Impulse);
    }
    public void Dash(Vector2 dir)
    {
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        //_rb.velocity = new Vector2(dir.normalized.x * playerData.dashSpeed,0);
        _rb.velocity = Vector2.zero;
        Vector2 force = new Vector2(dir.normalized.x * playerData.dashSpeed, 0);
        _rb.AddForce(force, ForceMode.Impulse);

        SetGravityScale(0);
        Physics.IgnoreLayerCollision(3, 9, true);
        bodyCollider.enabled = false;
    }
    public void EndDash()
    {
        Physics.IgnoreLayerCollision(3, 9, false);
        bodyCollider.enabled = true;
    }
    public void Slide()
    {
        //works the same as the Run but only in the y-axis
        float targetSpeed = 0;
        float speedDif = targetSpeed - _rb.velocity.y;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * playerData.slideAccel, playerData.slidePower) * Mathf.Sign(speedDif);
        _rb.AddForce(movement * Vector2.up, ForceMode.Force);
    }
    public void Climb()
    {
        //works the same as the Run but only in the y-axis
        float targetSpeed = InputHandler.instance.Movement.y * playerData.climbSpeed;
        float speedDif = targetSpeed - _rb.velocity.y;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * playerData.slideAccel, playerData.slidePower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(_rb.velocity.y, movement, 1);
        _rb.AddForce(movement * Vector2.up, ForceMode.Force);
    }
    public void WallJump(int dir)
    {
        //ensures we can't call a jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        #region Perform Wall Jump
        var yDiff = playerData.wallJumpForce.y - _rb.velocity.y;
        Vector2 force = new Vector2(playerData.wallJumpForce.x, yDiff);
        force.x *= dir; //apply force in opposite direction of wall

        if (Mathf.Sign(_rb.velocity.x) != Mathf.Sign(force.x))
            force.x -= _rb.velocity.x;

        if (_rb.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= _rb.velocity.y;

        _rb.AddForce(force, ForceMode.Impulse);
        #endregion
    }
    #endregion

    #region Combat Methods
    public void Attack()
    {
        //_rb.velocity = Vector2.zero;
        SetGravityScale(0);
        _rb.AddForce(new Vector2(0, -_rb.velocity.y), ForceMode.Impulse);
        attackCount++;
        ResetAttackCooldown();

        if (attackCount > playerData.maxAttackCount-1)
            ResetAttackCount();
    }
    public void ResetAttackCount()
    {
        attackCount = 0;
    }
    public void EndAttack()
    {
        SetGravityScale(playerData.fallGravityMult);
        attackState.IsAttackEnd();
    }
    private void ResetAttackCooldown()
    {
        AttackCooldown = playerData.attackCooldown;
    }
    public void AttackAnimationHandler(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if(e.Data == startCollision.EventData)
        {
            attackCollision.SetActive(true);
        }
        else if(e.Data == endCollision.EventData)
        {
            attackCollision.SetActive(false);
        }
        else if(e.Data == endAttack.EventData)
        {
            EndAttack();
        }
    }
    public void KnockBack(float forceScale)
    {
        Vector2 force = new Vector2(playerData.knockBackForce.x, playerData.knockBackForce.y);
        force.x *= -1f * Mathf.Sign(touchedCollision.GetComponent<AttackCollision>().owner.transform.position.x - transform.position.x);
        _rb.AddForce(force * forceScale, ForceMode.Impulse);
    }
    public void Hurt(Collider collider)
    {
        touchedCollision = collider;
        Debug.Log("Hurt");
        LastKnockBackTime = playerData.knockBackTime;
        stateMachine.ChangeState(knockBackState);
    }
    #endregion

    #region Spacial Abilitys

    private void ChangeWorld()
    {
        if (IsSetFire == false)
        {
            _changeWorldCheck.transform.SetParent(transform);
            _changeWorldCheck.transform.localPosition = Vector2.zero;
        }
        else if(IsSetFire == true)
        {
            _changeWorldCheck.transform.SetParent(Fire.transform);
            _changeWorldCheck.transform.localPosition = Vector2.zero;
        }
        _changeWorldCheck.gameObject.SetActive(!_changeWorldCheck.gameObject.activeSelf);
        _changeWorldCheck.transform.localScale = new Vector2(_minRadius, _minRadius);
        InvokeRepeating("TranslateRadius", 0f, Time.deltaTime);
    }
    private void TranslateRadius()
    {
        if (_changeWorldCheck.transform.localScale.x < playerData.maxChangeWorldRadius && isPressChangeWorld)
            _changeWorldCheck.transform.localScale = new Vector2
                (_changeWorldCheck.transform.localScale.x + Time.deltaTime * 7f, 
                _changeWorldCheck.transform.localScale.y + Time.deltaTime * 7f);
        else if(_changeWorldCheck.transform.localScale.x > playerData.minChangeWorldRadius && !isPressChangeWorld)
            _changeWorldCheck.transform.localScale = new Vector2
                (_changeWorldCheck.transform.localScale.x - Time.deltaTime * 1f,
                _changeWorldCheck.transform.localScale.y - Time.deltaTime * 1f);
        else if(!isPressChangeWorld)
        {
            IsInChangeWorld = false;
            CancelInvoke("TranslateRadius");
            EventManager.instance.EndChangeWorldEvent.Invoke();
            _changeWorldCheck.gameObject.SetActive(false);
        }
    }
    private void SetFire()
    {
        if (!IsInChangeWorld)
        {
            if (IsSetFire == false)
            {
                Fire.SetActive(true);
                Fire.transform.position = transform.position;
                IsSetFire = true;
            }
            else if (IsSetFire == true)
            {
                Fire.SetActive(false);
                IsSetFire = false;
            }
        }
    }

    #endregion

}