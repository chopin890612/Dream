using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStatus
{ 
    public Status<float> attack{ get; private set; }
    public Status<float> maxHealth { get; private set; }
    public Status<float> currentHealth { get; private set; }
    public Status<int> attackOwner { get; private set; }
    public CombatStatus(CombatData intiData)
    {
        attack = new Status<float>(intiData.attackDamage);
        maxHealth = new Status<float>(intiData.maxHealth);
        attackOwner =new Status<int>(intiData.attackOwner);

        currentHealth = new Status<float>(intiData.maxHealth);
    }

    public float Hurt(float damage)
    {
        return currentHealth.ChangeValue(currentHealth.value - damage);
    }
    public float Heal(float heal)
    {
        if(currentHealth.value + heal > maxHealth.value)
        {
            return currentHealth.ChangeValue(maxHealth.value);
        }
        else
        {
            return currentHealth.ChangeValue(currentHealth.value + heal);
        }
    }
}
