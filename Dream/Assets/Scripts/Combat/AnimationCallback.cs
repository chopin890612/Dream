using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    public TestPlayer player;
    public Enemy enemy;
    public void EndAttack()
    {
        if(player != null)
            player.EndAttack();
        if(enemy != null)
            enemy.EndAttack();
    }
}
