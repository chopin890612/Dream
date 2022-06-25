using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newPlayerData", menuName = "Bang's Things/ScriptObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movment")]
    public float moveSpeed = 10f;

    [Header("Jump")]
    public float jumpSpeed = 10f;
    public float maxJumpTime = 0.5f;
    public float minJumpMutiplyer = 0.5f;
    public int jumpAmount = 1;
    public float jumpBufferTime = 0.1f;

    [Header("Ground Check")]
    public float anchorOffset = 0f;
    public float groundDetectRadius = 0.2f;
    public float groundDetectDistance = 0.6f;

    [Header("Wall Check")]
    public float wallDetectDistance = 0.1f;

    [Header("World Value")]
    public Vector2 gravity = new Vector2(0, -9.81f);
}
