using _Main.Scripts.GamePlay.ActionSystem;
using _Main.Scripts.GamePlay.HealthSystem;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class RangedAttackBase : AttackBase
    {
        [SerializeField] protected ProjectileBase projectilePrefab;

        [Header("Shooters")]
        [SerializeField] protected Shooter shooter;

        [Header("Attack Params")]
        [SerializeField] protected IDamageable _caster;
        [SerializeField] protected DamageDealerType damageDealerType;
        [SerializeField] protected float shootDelay = 0.5f;

        protected bool isActive;

        private void Awake()
        {
            CurrentComboAnimationData = comboDatas[0].AttackAnimationData;
            CurrentComboDamageData = comboDatas[0].AttackDamageData;
        }

        public void Init(IAction action, IDamageable caster)
        {
            base.Init(action);
            _caster = caster;
        }

        protected override void DoOnActionStart()
        {
            isActive = true;
        }

        protected override void DoOnActionEnd()
        {
            isActive = false;
            OnAttackCompleted?.Invoke();
            IsOnCooldown = true;
            DOVirtual.DelayedCall(GeneralAttackCooldown, () => IsOnCooldown = false);
        }

        protected override void DoOnActionCanceled()
        {
            isActive = false;
        }

        protected virtual void Shoot()
        {

        }
    }
}
