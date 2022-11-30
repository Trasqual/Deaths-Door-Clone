using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class PlayerBowAttack : RangedAttackBase
    {
        [SerializeField] private ProjectileBase chargedProjectilePrefab;

        [Header("Visuals")]
        [SerializeField] private GameObject bow;

        [Header("Attack Params")]
        [SerializeField] private float windUpTime = 1f;
        [SerializeField] private float maxChargeTime = 2f;
        private float windUpCounter;
        private float dmgMultiplier = 1f;

        protected override void DoOnActionStart()
        {
            base.DoOnActionStart();
            bow.SetActive(true);
        }

        protected override void DoOnActionEnd()
        {
            base.DoOnActionEnd();

            if (windUpCounter >= windUpTime)
                Shoot();

            windUpCounter = 0f;
            dmgMultiplier = 1f;
            bow.SetActive(false);
        }

        protected override void DoOnActionCanceled()
        {
            base.DoOnActionCanceled();
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

            if (windUpCounter >= maxChargeTime)
            {
                dmgMultiplier = 2f;
            }
        }

        protected override void Shoot()
        {
            base.Shoot();
            OnAttackCompleted?.Invoke();
            shooter.Shoot(windUpCounter >= maxChargeTime ? chargedProjectilePrefab : projectilePrefab, CurrentComboDamageData.damage * dmgMultiplier, damageDealerType, _caster, out ProjectileBase shotProjectile);
        }
    }
}