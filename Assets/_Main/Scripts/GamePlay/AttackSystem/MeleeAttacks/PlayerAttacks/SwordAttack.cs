using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class SwordAttack : MeleeAttackBase
    {
        [SerializeField] GameObject swordVisual;

        protected override void DoOnActionStart()
        {
            base.DoOnActionStart();
            swordVisual.SetActive(true);
        }

        protected override void EndAttack()
        {
            base.EndAttack();
            swordVisual.SetActive(false);
        }
    }
}