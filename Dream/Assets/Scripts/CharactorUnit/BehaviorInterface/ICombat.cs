using UnityEngine;

public interface ICombat
{
    void Attack();
    void ResetAttackCooldown();
    void EndAttack();
    void Hurt(Collider collider);
    void KnockBack(float forceScale);
}
