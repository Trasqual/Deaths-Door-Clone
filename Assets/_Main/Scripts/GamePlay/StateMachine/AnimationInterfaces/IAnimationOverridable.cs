using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.StateMachineSystem;
using UnityEngine;

public interface IAnimationOverridable : IAnimation
{
    RuntimeAnimatorController OriginalController { get; }
    void SetAnimatorOverrideController();
    void ResetAnimatorController();
}
