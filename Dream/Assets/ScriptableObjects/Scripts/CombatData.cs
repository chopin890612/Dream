using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCombatData", menuName = "Bang's Things/ScriptObjects/CombatData")]
public class CombatData : ScriptableObject
{
    public float attackDamage;
    public float maxHealth;
    public int attackOwner;
}
