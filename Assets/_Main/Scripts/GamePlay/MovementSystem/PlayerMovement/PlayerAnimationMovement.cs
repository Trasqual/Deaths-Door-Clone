using UnityEngine;

namespace _Main.Scripts.GamePlay.MovementSystem
{
    public class PlayerAnimationMovement : AnimationMovementBase
    {
        CharacterController characterController;

        protected override void Start()
        {
            base.Start();
            characterController = GetComponentInParent<CharacterController>();
        }

        protected override void OnAnimatorMove()
        {
            if (!isActive) return;
            characterController.Move(anim.deltaPosition);
        }
    }
}