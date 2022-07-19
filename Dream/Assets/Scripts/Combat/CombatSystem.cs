using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatSystem : MonoBehaviour
{
    public CombatData combatData;
    public GameObject parent;

    public CombatStatus data { get; private set; }

    public UnityEvent<Collider> hurtEvent;
    public UnityEvent<Collider> healEvent;
    void Start()
    {
        data = new CombatStatus(combatData);
        hurtEvent.AddListener(DieCheck);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DieCheck(Collider collider)
    {
        if(data.currentHealth.value <= 0f)
        {
            Debug.LogWarning("Die!!");
            Destroy(parent);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackCollider"))
        {
            if (other.gameObject.GetComponent<AttackCollision>().data.attackOwner.value != data.attackOwner.value)
            {
                data.Hurt(other.gameObject.GetComponent<AttackCollision>().data.attack.value);                
                if(hurtEvent != null)
                    hurtEvent.Invoke(other);

                Debug.Log(data.currentHealth.value);
            }
        }
        else if (other.CompareTag("Heal"))
        {
            if (data.currentHealth.value != data.maxHealth.value)
            {
                data.Heal(other.GetComponent<HealPotion>().healValue);
                if(healEvent != null)
                    healEvent.Invoke(other);
            }

            Debug.Log(data.currentHealth.value);
        }
    }
}
