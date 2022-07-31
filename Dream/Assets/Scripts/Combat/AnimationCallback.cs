using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    public TestPlayer player;
    public Enemy enemy;
    public Boss boss;
    public void EndAttack()
    {
        if (player != null)
            player.EndAttack();
        else if (enemy != null)
            enemy.EndAttack();
        else if (boss != null)
            boss.EndAttack();
    }
}
