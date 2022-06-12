using UnityEngine;

public abstract class ActionIndicatorBase : MonoBehaviour
{
    [SerializeField] protected ActionBase action;

    protected abstract void Activate();
    protected abstract void Deactivate();
    protected abstract void DoOnActionPerformed();

    private void Start()
    {
        if (action)
        {
            action.OnActionStarted += Activate;
            action.OnActionEnded += Deactivate;
            action.OnActionPerformed += DoOnActionPerformed;
        }
    }
}
