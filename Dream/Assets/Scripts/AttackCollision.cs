using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AttackCollision : MonoBehaviour
{
    public SkeletonAnimation sA;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            sA.timeScale = 0;
            GameManager.instance.DoForSeconds(() => sA.timeScale = 1, 0.1f);
        }
    }
    
}
