using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public interface IAnimation
    {
        int HashCode { get; }
        Animator Animator { get; }
        void PlayAnimation();
        void StopAnimation();
    }
}