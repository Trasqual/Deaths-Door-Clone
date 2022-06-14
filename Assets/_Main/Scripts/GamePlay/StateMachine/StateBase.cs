using UnityEngine;

public abstract class StateBase : MonoBehaviour
{
    protected StateMachine stateMachine;

    public StateBase(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void CancelState();
}
