using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCombatData", menuName = "Bang's Things/ScriptObjects/CombatData/CombatData")]
public class CombatData : ScriptableObject
{
    public float attackDamage;
    public int attackOwner;
    public float maxHealth;
    public float maxSuperArmor; 
}
