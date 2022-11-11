using _Main.Scripts.GamePlay.AttackSystem.RangedAttacks;
using DG.Tweening;
using UnityEngine;

public class RangedEnemyAttack : RangedAttackBase
{
    [SerializeField] private float shootDelay = 0.5f;

    protected override void DoOnActionEnd()
    {
        base.DoOnActionEnd();
        Shoot();
    }

    protected override void Shoot()
    {
        DOVirtual.DelayedCall(shootDelay, () =>
        {
            base.Shoot();
            shooter.Shoot(projectilePrefab, CurrentComboDamageData.damage, damageDealerType, _caster);
        });
    }
}
