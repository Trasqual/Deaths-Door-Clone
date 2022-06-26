using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.MeleeAttacks
{
    public class UnarmedAttack : AttackBase
    {
        [SerializeField] ParticleSystem unarmedParticle;

        public override void DoOnActionStart()
        {
            //play particle
        }

        public override void DoOnActionEnd()
        {
            //damage?
        }

        public override void DoOnActionCanceled()
        {

        }
    }
}
