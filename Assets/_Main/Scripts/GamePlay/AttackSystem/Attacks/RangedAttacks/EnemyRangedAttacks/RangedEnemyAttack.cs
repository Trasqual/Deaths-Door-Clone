using DG.Tweening;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class RangedEnemyAttack : RangedAttackBase
    {
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
                shooter.Shoot(projectilePrefab, CurrentComboDamageData.damage, damageDealerType, _caster, out ProjectileBase shotProjectile);
            });
        }
    }
}