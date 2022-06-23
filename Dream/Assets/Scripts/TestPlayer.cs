using System.Collections;
using UnityEngine;
using Bang.StateMachine;
using Bang.StateMachine.PlayerMachine;



public class TestPlayer : MonoBehaviour
{
    [Header("PlayerData")]
    public ObjectData playerData;

    public int faceDirection = 1;


    public StateMachine<TestPlayer, ObjectData> stateMachine { get; private set; }
    public IdleState idleState { get; private set; }
    public MoveState moveState { get; private set; }


    public InputHandler inputActions { get; private set; }
    public Rigidbody _rb;
    private float _flyTime = 0f;
    private Vector3 _gravity = new Vector3(0, -50f, 0);
    private float _gravityScale = 1f;

    private void Awake()
    {
        stateMachine = new StateMachine<TestPlayer, ObjectData>();
        idleState = new IdleState(this, stateMachine, playerData);
        moveState = new MoveState(this, stateMachine, playerData);
    }
    private void Start()
    {
        inputActions = GetComponent<InputHandler>();
        _rb = GetComponent<Rigidbody>();
        stateMachine.Initalize(idleState);
    }
    private void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, playerData.GetData("anchorOffset").GetValue<float>(), 0), playerData.GetData("groundDetectRadius").GetValue<float>());
    }

    public void CheckIfShouldFlip()
    {
        if (inputActions.NormInputX != 0 && inputActions.NormInputX != faceDirection)
            Flip();
    }

    private void Flip()
    {
        faceDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
}
    