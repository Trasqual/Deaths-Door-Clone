
using UnityEngine;

public class EnemyAnimationMovement : AnimationMovementBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnAnimatorMove()
    {
        if (!isActive) return;
        transform.parent.position += anim.deltaPosition;
    }
}
