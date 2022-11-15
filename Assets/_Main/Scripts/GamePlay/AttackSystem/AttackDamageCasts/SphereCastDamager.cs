using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class SphereCastDamager
    {
        public SphereCastDamager(Vector3 startPos, float radius, Vector3 direction, float range, int damage, DamageDealerType dmgDealerType, out int hitCount)
        {
            hitCount = CastCollider(startPos, radius, direction, range, damage, dmgDealerType);
        }

        public int CastCollider(Vector3 startPos, float radius, Vector3 direction, float range, int damage, DamageDealerType dmgDealerType)
        {
            var colliders = Physics.SphereCastAll(startPos, radius, direction, range);
            var hitColliders = 0;

            foreach (var collider in colliders)
            {
                if (collider.transform.TryGetComponent(out IDamageable damagable))
                {
                    if (damagable.TakeDamage(damage, dmgDealerType))
                    {
                        hitColliders++;
                    }
                }
            }

            return hitColliders;
        }
    }
}