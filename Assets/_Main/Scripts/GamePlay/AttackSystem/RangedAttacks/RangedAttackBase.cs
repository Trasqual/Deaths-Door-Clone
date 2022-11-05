using _Main.Scripts.GamePlay.ActionSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class RangedAttackBase : AttackBase
    {
        [Header("Visuals")]
        [SerializeField] private GameObject bow;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Projectile chargedProjectilePrefab;

        [Header("Shooters")]
        [SerializeField] private Shooter shooter;

        [Header("Attack Params")]
        [SerializeField] IDamageable _caster;
        [SerializeField] DamageDealerType damageDealerType;
        [SerializeField] private float windUpTime = 1f;
        [SerializeField] private float maxChargeTime = 2f;
        private float dmgMultiplier;
        private float windUpCounter;

        private bool isActive;

        public void Init(IAction action, IDamageable caster)
        {
            base.Init(action);
            _caster = caster;
        }

        protected override void DoOnActionStart()
        {
            bow.SetActive(true);
            CurrentComboAnimationData = comboDatas[0].AttackAnimationData;
            isActive = true;
        }

        protected override void DoOnActionEnd()
        {
            isActive = false;

            if (windUpCounter >= windUpTime)
                Shoot();

            windUpCounter = 0f;
            dmgMultiplier = 1f;
            bow.SetActive(false);
        }

        protected override void DoOnActionCanceled()
        {
            isActive = false;
            windUpCounter = 0f;
            dmgMultiplier = 1f;
            bow.SetActive(false);
        }

        private void Update()
        {
            if (isActive)
            {
                ChargeAttack();
            }
        }

        private void ChargeAttack()
        {
            windUpCounter += Time.deltaTime;

            if(windUpCounter >= maxChargeTime)
            {
                dmgMultiplier = 2f;
            }
        }

        private void Shoot()
        {
            shooter.Shoot(windUpCounter >= maxChargeTime ? chargedProjectilePrefab : projectilePrefab, dmgMultiplier, damageDealerType, _caster);
        }
    }
}
