using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour
{
    public CombatData combatData;
    public GameObject parent;
    public ParticleSystem attackPartical;
    public float lastAttackSeed;

    public CombatStatus currentData { get; private set; }

    public UnityEvent<Collider> hurtEvent;
    public UnityEvent<Collider> healEvent;
    public UnityEvent<Collider> superArmorBreakEvent;

    [SerializeField] private float lastBeingAttackTime;
    void Start()
    {
        currentData = new CombatStatus(combatData);
        hurtEvent.AddListener(DieCheck);
    }

    void Update()
    {
        
    }

    public void DieCheck(Collider collider)
    {
        if(currentData.currentHealth.value <= 0f)
        {
            Debug.LogWarning("Die!!");
            Destroy(parent);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackCollider"))
        {
            var attack = other.gameObject.GetComponent<AttackCollision>();
            if ((attack.data.attackOwner.value != currentData.attackOwner.value) && attack.attackSeed != lastAttackSeed)
            {
                lastAttackSeed = attack.attackSeed;
                currentData.Hurt(attack.data.attack.value);
                attackPartical.Play();
                if (hurtEvent != null)
                {
                    hurtEvent.Invoke(other);
                }

                currentData.DecreaseSuperArmor(attack.data.attack.value);
                if(superArmorBreakEvent != null && currentData.currentSuperArmor.value <= 0)
                {
                    superArmorBreakEvent.Invoke(other);
                    currentData.ResetSuperArmor();
                }

                Debug.Log($"{parent.name} Current Health: {currentData.currentHealth.value}");
                Debug.Log($"{parent.name} Current SuperArmor: {currentData.currentSuperArmor.value}");
            }
        }
        else if (other.CompareTag("Heal"))
        {
            if (currentData.currentHealth.value != currentData.maxHealth.value)
            {
                currentData.Heal(other.GetComponent<HealPotion>().healValue);
                if(healEvent != null)
                    healEvent.Invoke(other);
            }

            Debug.Log(currentData.currentHealth.value);
        }
    }
}
