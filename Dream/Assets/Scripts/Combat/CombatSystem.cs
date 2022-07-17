using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatSystem : MonoBehaviour
{
    public CombatData combatData;

    public CombatStatus data { get; private set; }

    public UnityEvent<Collider> hurtEvent;
    void Start()
    {
        data = new CombatStatus(combatData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackCollider"))
        {
            if (other.gameObject.GetComponent<AttackCollision>().data.attackOwner != data.attackOwner)
            {
                Debug.Log(data.Hurt(other.gameObject.GetComponent<AttackCollision>().data.attack.value));
                if(hurtEvent != null)
                    hurtEvent.Invoke(other);
            }
        }
    }
}
