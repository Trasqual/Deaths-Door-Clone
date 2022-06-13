using UnityEngine;

public class UnarmedAttack : MeleeAction
{
    AnimationBase anim;

    private void Awake()
    {
        anim = GetComponentInParent<AnimationBase>();
    }

    public override void PerformAction()
    {
        if (overrideChecker != null)
        {
            if (overrideChecker.CanOverride())
            {
                anim.PlayAttackAnim();
            }
        }
    }
}
