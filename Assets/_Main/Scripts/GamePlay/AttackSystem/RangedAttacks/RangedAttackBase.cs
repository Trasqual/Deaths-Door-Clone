using _Main.Scripts.GamePlay.ActionSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class RangedAttackBase : AttackBase
    {
        [SerializeField] protected Projectile projectilePrefab;        

        [Header("Shooters")]
        [SerializeField] protected Shooter shooter;

        [Header("Attack Params")]
        [SerializeField] protected IDamageable _caster;
        [SerializeField] protected DamageDealerType damageDealerType;
        protected float dmgMultiplier = 1;

        protected bool isActive;

        public void Init(IAction action, IDamageable caster)
        {
            base.Init(action);
            _caster = caster;
        }

        protected override void DoOnActionStart()
        {
            CurrentComboAnimationData = comboDatas[0].AttackAnimationData;
            isActive = true;
        }

        protected override void DoOnActionEnd()
        {
            isActive = false;
        }

        protected override void DoOnActionCanceled()
        {
            isActive = false;
        }

        protected virtual void Shoot()
        {
            OnAttackCompleted?.Invoke();
        }
    }
}
