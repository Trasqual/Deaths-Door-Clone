using UnityEngine;

public abstract class ActionIndicatorBase : MonoBehaviour
{
    protected abstract void Activate();
    protected abstract void Deactivate();
    protected abstract void DoOnActionPerformed();

    public void Init(IAction action)
    {
        action.OnActionStart += Activate;
        action.OnActionEnd += Deactivate;
    }
}
