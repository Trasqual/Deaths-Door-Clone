using System.Collections;
using UnityEngine;

public class DamageDealingLaserCaster : LaserCaster
{
    [SerializeField] DamageDealerType _damageDealerType;
    [SerializeField] int _damage = 1;
    [SerializeField] float _dotTimer = 0.5f;

    Transform prevHitTarget;
    IEnumerator DoT;

    public override void CastLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, _distance, _mask, QueryTriggerInteraction.Ignore);
        if (hit.transform != null)
        {
            _spawnedBeam.UpdateBeam(new Vector3[] { transform.position, (hit.point - transform.forward * 0.1f) });
            if (hit.transform.TryGetComponent(out IDamageable damagable))
            {
                if (hit.transform != prevHitTarget)
                {
                    prevHitTarget = hit.transform;
                    KillDot();
                    DoT = DotCo(damagable);
                    StartCoroutine(DoT);
                }
            }
            else
            {
                KillDot();
                prevHitTarget = null;
            }
        }
        else
        {
            _spawnedBeam.UpdateBeam(new Vector3[] { transform.position, transform.position + transform.forward * _distance });
            KillDot();
            prevHitTarget = null;
        }
    }

    private IEnumerator DotCo(IDamageable damagable)
    {
        while (true)
        {
            damagable.TakeDamage(_damage, _damageDealerType);
            yield return new WaitForSeconds(_dotTimer);
        }
    }

    private void KillDot()
    {
        if (DoT != null)
        {
            StopCoroutine(DoT);
        }
    }
}
