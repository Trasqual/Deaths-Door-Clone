using _Main.Scripts.GamePlay.AnimationSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerAnimation : AnimationBase
    {
        private static readonly int AttackHash = Animator.StringToHash("attack");
        private static readonly int IsAimingHash = Animator.StringToHash("isAiming");
        private static readonly int IsCanceledHash = Animator.StringToHash("isCanceled");
        private static readonly int RollHash = Animator.StringToHash("roll");

        public void PlayAttackAnim(int comboCount)
        {
            _animator.SetTrigger(AttackHash);
        }
        public void PlayAimAnim(bool isAiming, bool isCanceled)
        {
            _animator.SetBool(IsAimingHash, isAiming);
            _animator.SetBool(IsCanceledHash, isCanceled);
        }
        public void PlayRollAnim()
        {
            _animator.SetTrigger(RollHash);
        }

        public void PlayDamageTakenAnim()
        {
            _animator.SetTrigger("takeDamage");
        }
    }
}