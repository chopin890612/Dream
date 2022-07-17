using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AttackCollision : MonoBehaviour
{
    public SkeletonAnimation sA;
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
            if (sA != null)
            {
                sA.timeScale = 0;
                GameManager.instance.DoForSeconds(() => sA.timeScale = 1, 0.1f);
            }
        }
    }
    
}
