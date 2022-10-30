using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

public interface IAnimationOverridable : IAnimation
{
    public RuntimeAnimatorController OriginalController { get; }
    public void SetAnimatorOverrideController();
    public void ResetAnimatorController();
}
