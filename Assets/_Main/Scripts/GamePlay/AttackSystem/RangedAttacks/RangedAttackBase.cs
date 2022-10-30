using _Main.Scripts.GamePlay.ActionSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class RangedAttackBase : AttackBase
    {
        [Header("Visuals")]
        [SerializeField] private GameObject _bow;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Projectile _chargedProjectilePrefab;

        [Header("Shooters")]
        [SerializeField] private Shooter _shooter;

        [Header("Attack Params")]
        [SerializeField] IDamageable _caster;
        [SerializeField] DamageDealerType _damageDealerType;
        [SerializeField] private float _initialChargeDelay = 0.5f;
        [SerializeField] private float _maxChargeTime = 3f;
        [SerializeField] private float _minDmgMultiplier = 1f;
        [SerializeField] private float _maxDmgMultiplier = 2f;
        private float _chargeDelayDuration;
        private float _chargeDuration;
        private float _dmgMultiplier;

        private bool _isActive;

        public void Init(IAction action, IDamageable caster)
        {
            base.Init(action);
            _caster = caster;
        }

        protected override void DoOnActionStart()
        {
            _bow.SetActive(true);
            CurrentComboAnimationData = _comboDatas[0].AttackAnimationData;
            _isActive = true;
        }

        protected override void DoOnActionEnd()
        {
            _isActive = false;
            Shoot();
            _chargeDelayDuration = 0f;
            _chargeDuration = 0f;
            _dmgMultiplier = 1f;
            _bow.SetActive(false);
        }

        protected override void DoOnActionCanceled()
        {
            _isActive = false;
            _chargeDelayDuration = 0f;
            _chargeDuration = 0f;
            _dmgMultiplier = 1f;
            _bow.SetActive(false);
        }

        private void Update()
        {
            if (_isActive)
            {
                ChargeAttack();
            }
        }

        private void ChargeAttack()
        {
            if (_chargeDelayDuration <= _initialChargeDelay)
            {
                _chargeDelayDuration += Time.deltaTime;
            }
            else
            {
                if (_chargeDuration <= _maxChargeTime)
                {
                    _chargeDuration += Time.deltaTime;
                    var t = Mathf.InverseLerp(0f, _maxChargeTime, _chargeDuration);
                    _dmgMultiplier = Mathf.Lerp(_minDmgMultiplier, _maxDmgMultiplier, t);
                }
            }
        }

        private void Shoot()
        {
            //shooter.transform.position = bow.transform.position;
            //shooter.transform.forward = transform.forward;
            _shooter.Shoot(_chargeDuration > 0 ? _chargedProjectilePrefab : _projectilePrefab, _dmgMultiplier, _damageDealerType, _caster);
        }
    }
}
