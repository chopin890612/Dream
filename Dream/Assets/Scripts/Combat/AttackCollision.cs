using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AttackCollision : MonoBehaviour
{
    public SkeletonAnimation sA;
    public Animator animator;
    public CombatData combatData;

    public CombatStatus data { get; private set; }
    private void Start()
    {
        data = new CombatStatus(combatData);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            if (other.gameObject.GetComponent<CombatSystem>().data.attackOwner.value != data.attackOwner.value)
            {
                if (sA != null)
                {
                    sA.timeScale = 0;
                    GameManager.instance.DoForSeconds(() => sA.timeScale = 1, 0.1f);
                }
                if (animator != null)
                {
                    animator.speed = 0;
                    GameManager.instance.DoForSeconds(() => animator.speed = 1, 0.1f);
                }
            }
        }
    }
    
}
