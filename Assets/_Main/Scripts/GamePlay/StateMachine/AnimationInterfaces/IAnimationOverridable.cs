using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

public interface IAnimationOverridable : IAnimation
{
    RuntimeAnimatorController OriginalController { get; }
    void SetAnimatorOverrideController();
    void ResetAnimatorController();
}
