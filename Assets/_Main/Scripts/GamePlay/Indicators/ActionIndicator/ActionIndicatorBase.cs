using _Main.Scripts.GamePlay.ActionSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Indicators.ActionIndicator
{
    public abstract class ActionIndicatorBase : MonoBehaviour
    {
        protected abstract void Activate();
        protected abstract void Deactivate();
        protected abstract void DoOnActionPerformed();

        public void Init(IAction action)
        {
            action.OnActionStart += Activate;
            action.OnActionEnd += Deactivate;
            action.OnActionCanceled += Deactivate;
        }
    }
}
