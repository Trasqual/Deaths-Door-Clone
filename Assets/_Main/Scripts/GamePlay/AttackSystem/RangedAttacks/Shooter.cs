using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class Shooter : MonoBehaviour
    {
        private Collider _col;

        private void Awake()
        {
            _col = GetComponentInParent<Collider>();
        }

        public void Shoot(Projectile projectilePrefab, float dmgMultiplier, DamageDealerType damageDealerType, IDamageable caster)
        {
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.Init(dmgMultiplier, damageDealerType, caster);
            Physics.IgnoreCollision(_col, projectile.Col);
        }
    }
}
