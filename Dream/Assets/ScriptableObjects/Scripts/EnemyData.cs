using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Bang's Things/ScriptObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Drag")]
    public float dragAmount; //drag in air
    public float frictionAmount; //drag on ground

    [Header("Run")]
    public float runMaxSpeed;
    public float runAccel;
    public float runDeccel;
    [Space(5)]
    [Range(.5f, 2f)] public float accelPower;
    [Range(.5f, 2f)] public float stopPower;
    [Range(.5f, 2f)] public float turnPower;
    [Range(0f, 90f)] public float maxSlopeAngle;
}