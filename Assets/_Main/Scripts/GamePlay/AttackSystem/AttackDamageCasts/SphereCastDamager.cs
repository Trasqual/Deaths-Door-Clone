using UnityEngine;

public class SphereCastDamager
{
    public SphereCastDamager(Vector3 startPos, float radius, Vector3 direction, float range, int damage, DamageDealerType dmgDealerType)
    {
        CastCollider(startPos, radius, direction, range, damage, dmgDealerType);
    }

    public void CastCollider(Vector3 startPos, float radius, Vector3 direction, float range, int damage, DamageDealerType dmgDealerType)
    {
        var colliders = Physics.SphereCastAll(startPos, radius, direction, range);
        foreach (var collider in colliders)
        {
            if (collider.transform.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage, dmgDealerType);
            }
        }
    }
}
