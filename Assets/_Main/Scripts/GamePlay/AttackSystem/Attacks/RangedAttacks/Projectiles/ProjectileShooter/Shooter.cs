using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class Shooter : MonoBehaviour
    {
        public void Shoot(ProjectileBase projectilePrefab, float damage, DamageDealerType damageDealerType, IDamageable caster, out ProjectileBase shotProjectile)
        {
            shotProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            shotProjectile.Init(damage, damageDealerType, caster);
        }
    }
}
