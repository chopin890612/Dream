using System.Collections;
using UnityEngine;
using Bang.StateMachine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseCharactor : MonoBehaviour
{
    #region State Parameters

    #endregion

    #region Components
    private Rigidbody _rb;
    #endregion

    #region Unity LifeCycles
    private void Awake()
    {
        
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
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
    public virtual void Run(float lerpAmount, bool walkSlope, float multiple)
    {

    }
    #endregion

    #region Combat Methods
    #endregion
}