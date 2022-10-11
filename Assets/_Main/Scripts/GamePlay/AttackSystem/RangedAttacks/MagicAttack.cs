using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class MagicAttack : AttackBase
    {
        protected override void DoOnActionStart()
        {
            Debug.Log("testing magic start");
        }

        protected override void DoOnActionEnd()
        {
            Debug.Log("testing magic end");
        }

        protected override void DoOnActionCanceled()
        {
            Debug.Log("testing magic canceled");
        }
    }
}
