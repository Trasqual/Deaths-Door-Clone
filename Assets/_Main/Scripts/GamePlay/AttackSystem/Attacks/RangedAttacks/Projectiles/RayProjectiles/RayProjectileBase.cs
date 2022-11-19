using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public abstract class RayProjectileBase : ProjectileBase
    {
        [SerializeField] protected Beam _beam;

        protected virtual void Awake()
        {
            _beam.SetMaxDistance(_maxTravelDistance);
        }

        protected abstract void DoOnHit(RaycastHit hit);

        protected abstract void DoOnNoHit();

        public override void KillProjectile()
        {
            _beam.DeActivate();
        }

        protected virtual void OnEnable()
        {
            _beam.OnBeamHit += DoOnHit;
            _beam.OnBeamNoHit += DoOnNoHit;
        }

        protected virtual void OnDisable()
        {
            _beam.OnBeamHit -= DoOnHit;
            _beam.OnBeamNoHit -= DoOnNoHit;
        }
    }
}
