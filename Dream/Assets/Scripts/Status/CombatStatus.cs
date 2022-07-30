using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStatus
{ 
    public Status<float> attack{ get; private set; }
    public Status<int> attackOwner { get; private set; }
    public Status<float> maxHealth { get; private set; }
    public Status<float> currentHealth { get; private set; }
    public Status<float> maxSuperArmor { get; private set; }
    public Status<float> currentSuperArmor { get; private set; }

    public CombatStatus(CombatData intiData)
    {
        attack = new Status<float>(intiData.attackDamage);
        attackOwner =new Status<int>(intiData.attackOwner);

        maxHealth = new Status<float>(intiData.maxHealth);
        currentHealth = new Status<float>(intiData.maxHealth);

        maxSuperArmor = new Status<float>(intiData.maxSuperArmor);
        currentSuperArmor = new Status<float>(intiData.maxSuperArmor);
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
    public float DecreaseSuperArmor(float damage)
    {
        return currentSuperArmor.ChangeValue(currentSuperArmor.value - damage);
    }
    public void ResetSuperArmor()
    {
        currentSuperArmor.ChangeValue(maxSuperArmor.value);
    }
}
