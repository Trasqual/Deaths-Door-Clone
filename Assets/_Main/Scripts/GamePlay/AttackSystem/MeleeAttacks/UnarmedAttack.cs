using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.MeleeAttacks
{
    public class UnarmedAttack : AttackBase
    {
        [SerializeField] ParticleSystem unarmedParticle;

        protected override void DoOnActionStart()
        {
            //play particle
        }

        protected override void DoOnActionEnd()
        {
            //damage?
        }

        protected override void DoOnActionCanceled()
        {

        }
    }
}
