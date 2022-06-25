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
    [Space(5)]

    [Header("Wall Settings")]
    [SerializeField] float wallDetectDistance = 1f;
    [SerializeField] Vector3 wallDetectRadius = new Vector3(1, 1, 1);
    [SerializeField] bool onWall = false;
    [SerializeField] bool isWallJumping = false;
    [SerializeField] float wallJumpSpeedx;
    [Space(5)]

    private InputMaster _inputActions;
    private Rigidbody _rb;
    private SkeletonAnimation _skeletonAnimation;
    private bool _isWallOnRight = true;
    private Transform _wallTransform;

    private void Start()
    {
        _inputActions = GetComponent<InputMaster>();
        _rb = GetComponent<Rigidbody>();
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public void StateCallback(string stateName)
    {
        switch (stateName)
        {
            default:
                Debug.LogWarning("Wrong state state name: " + stateName);
                break;
        }
    }

}
