using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class Shooter : MonoBehaviour
    {
        Collider col;

        private void Awake()
        {
            col = GetComponentInParent<Collider>();
        }

        public void Shoot(Projectile projectilePrefab, float dmgMultiplier, DamageDealerType damageDealerType)
        {
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.Init(dmgMultiplier, damageDealerType);
            Physics.IgnoreCollision(col, projectile.Col);
        }
    }
}
