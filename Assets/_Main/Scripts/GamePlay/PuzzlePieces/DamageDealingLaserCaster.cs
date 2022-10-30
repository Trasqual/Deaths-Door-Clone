using System.Collections;
using UnityEngine;

public class DamageDealingLaserCaster : LaserCaster, IDamageDealer
{
    [SerializeField] DamageDealerType _damageDealerType;
    [SerializeField] int _damage = 1;
    [SerializeField] float _dotTimer = 0.5f;

    private Transform _prevHitTarget;
    private IEnumerator _doT;

    public override void CastLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, _distance, _mask, QueryTriggerInteraction.Ignore);
        if (hit.transform != null)
        {
            _spawnedBeam.UpdateBeam(new Vector3[] { transform.position, (hit.point - transform.forward * 0.1f) });
            if (hit.transform.TryGetComponent(out IDamageable damagable))
            {
                if (hit.transform != _prevHitTarget)
                {
                    _prevHitTarget = hit.transform;
                    KillDot();
                    _doT = DotCo(damagable);
                    StartCoroutine(_doT);
                }
            }
            else
            {
                KillDot();
                _prevHitTarget = null;
            }
        }
        else
        {
            _spawnedBeam.UpdateBeam(new Vector3[] { transform.position, transform.position + transform.forward * _distance });
            KillDot();
            _prevHitTarget = null;
        }
    }

    private IEnumerator DotCo(IDamageable damagable)
    {
        while (true)
        {
            DealDamage(_damage, damagable, _damageDealerType);
            yield return new WaitForSeconds(_dotTimer);
        }
    }

    private void KillDot()
    {
        if (_doT != null)
        {
            StopCoroutine(_doT);
        }
    }

    public void DealDamage(int damage, IDamageable damagable, DamageDealerType damageDealerType)
    {
        damagable.TakeDamage(damage, _damageDealerType);
    }
}
