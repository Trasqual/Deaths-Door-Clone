using DG.Tweening;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class BossRangedAttack2 : RangedAttackBase
    {
        ProjectileBase projectile;
        Tween shootDelayTween;

        protected override void DoOnActionStart()
        {
            base.DoOnActionStart();
            Shoot();
        }

        protected override void DoOnActionEnd()
        {
            base.DoOnActionEnd();
            if (projectile != null)
                projectile.KillProjectile();
            OnAttackCompleted?.Invoke();
            IsOnCooldown = true;
            DOVirtual.DelayedCall(GeneralAttackCooldown, () => IsOnCooldown = false);
        }

        protected override void DoOnActionCanceled()
        {
            base.DoOnActionCanceled();
            if (projectile != null)
                projectile.KillProjectile();
            shootDelayTween?.Kill();
        }

        protected override void Shoot()
        {
            shootDelayTween = DOVirtual.DelayedCall(shootDelay, () =>
            {
                shooter.Shoot(projectilePrefab, CurrentComboDamageData.damage, damageDealerType, _caster, out projectile);
                projectile.transform.SetParent(shooter.transform);
            });
        }
    }
}