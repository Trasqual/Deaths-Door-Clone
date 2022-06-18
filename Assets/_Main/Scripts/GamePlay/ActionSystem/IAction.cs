using System;

public interface IAction
{
    public event Action OnActionStart;
    public event Action OnActionEnd;
    public event Action OnActionCanceled;
}
