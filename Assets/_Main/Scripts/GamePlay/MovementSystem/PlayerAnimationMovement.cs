using UnityEngine;

public class PlayerAnimationMovement : AnimationMovementBase
{
    private CharacterController _characterController;

    protected override void Start()
    {
        base.Start();
        _characterController = GetComponentInParent<CharacterController>();
    }

    protected override void OnAnimatorMove()
    {
        if (!isActive) return;
        _characterController.Move(_anim.deltaPosition);
    }
}
