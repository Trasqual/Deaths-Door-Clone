using UnityEngine;

public abstract class StateBase
{
    protected StateMachine stateMachine;
    protected int priority;

    public StateBase(int priority, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.priority = priority;
    }

    public int GetPriority() => priority;
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CancelState();
}
