using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.PuzzleSystem
{
    public class LaserColumn : MonoBehaviour, IDamageable
    {
        [SerializeField] EndlessLaserProjectile laserPrefab;
        [SerializeField] float damage = 1f;
        [SerializeField] DamageDealerType damageType;
        [SerializeField] DamageDealerType effectedByType;
        private Rotator _rotator;
        private Shooter[] shooters;
        private List<ProjectileBase> castLasers = new();

        public Action<float> OnDamageTaken { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action OnDeath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private void Awake()
        {
            _rotator = GetComponent<Rotator>();
            shooters = GetComponentsInChildren<Shooter>();
        }

        private void Start()
        {
            Activate();
        }

        public void Activate()
        {
            _rotator.enabled = true;
            castLasers.Clear();
            foreach (var shooter in shooters)
            {
                shooter.Shoot(laserPrefab, damage, damageType, this, out ProjectileBase laser);
                castLasers.Add(laser);
                laser.transform.SetParent(shooter.transform);
            }
        }

        public void DeActivate()
        {
            _rotator.enabled = false;
            foreach (var laser in castLasers)
            {
                laser.KillProjectile();
            }
        }

        public Transform GetTransform() => transform;

        public DamageDealerType GetEffectedByType() => effectedByType;

        public bool TakeDamage(float amount, DamageDealerType damageDealerType)
        {
            return false;
        }

        public void Die()
        {
            DeActivate();
        }

        public bool IsDead()
        {
            return false;
        }
    }
}