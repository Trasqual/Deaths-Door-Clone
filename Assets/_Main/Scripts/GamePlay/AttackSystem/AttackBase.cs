using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    [SerializeField] AnimatorOverrideController overrideController;

    public virtual void Init(IAction action)
    {
        action.OnActionStart += DoOnActionStart;
        action.OnActionEnd += DoOnActionEnd;
        action.OnActionCanceled += DoOnActionCanceled;
    }

    public abstract void DoOnActionStart();
    public abstract void DoOnActionEnd();
    public abstract void DoOnActionCanceled();
    public virtual AnimatorOverrideController GetOverrideController()
    {
        return overrideController;
    }
}
