using UnityEngine;

public abstract class RangedAttack : MonoBehaviour
{
    public virtual void Init(IAction action)
    {
        action.OnActionStart += DoOnAimStart;
        action.OnActionEnd += DoOnAimEnd;
        action.OnActionCanceled += DoOnAimCanceled;
    }

    public abstract void DoOnAimStart();
    public abstract void DoOnAimEnd();
    public abstract void DoOnAimCanceled();
}
