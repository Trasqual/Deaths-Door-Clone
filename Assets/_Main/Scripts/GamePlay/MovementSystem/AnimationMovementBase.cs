using UnityEngine;

public class AnimationMovementBase : MonoBehaviour
{
    protected Animator _anim;

    protected bool isActive;

    protected virtual void Start()
    {
        _anim = GetComponent<Animator>();
    }

    protected virtual void OnAnimatorMove()
    {
        if (!isActive) return;
    }

    public virtual void Activate()
    {
        isActive = true;
    }

    public virtual void DeActivate()
    {
        isActive = false;
    }
}
