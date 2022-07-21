using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    public TestPlayer player;
    public void EndAttack()
    {
        player.EndAttack();
    }
}
