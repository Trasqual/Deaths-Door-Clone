using System.Collections;
using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class EndlessLaserProjectile : RayProjectileBase
    {
        [SerializeField] float _dotTimer = 0.5f;

        Transform prevHitTarget;
        IEnumerator DoT;

        protected override void DoOnHit(RaycastHit hit)
        {
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
                DoOnNoHit();
            }
        }

        protected override void DoOnNoHit()
        {
            if (prevHitTarget != null)
            {
                KillDot();
                prevHitTarget = null;
            }
        }

        private IEnumerator DotCo(IDamageable damagable)
        {
            var waitForSec = new WaitForSeconds(_dotTimer);
            while (true)
            {
                damagable.TakeDamage(_damage, _damageDealerType);
                yield return waitForSec;
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
}
