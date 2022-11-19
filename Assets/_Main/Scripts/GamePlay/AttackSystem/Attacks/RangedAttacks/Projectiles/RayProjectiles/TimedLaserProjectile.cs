using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class TimedLaserProjectile : EndlessLaserProjectile
    {
        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(TimerCo());
        }

        private IEnumerator TimerCo()
        {
            yield return new WaitForSeconds(_lifeTime);
            _beam.DeActivate();
        }
    }
}