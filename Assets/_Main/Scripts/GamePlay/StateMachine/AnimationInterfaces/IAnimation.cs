using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachineSystem
{
    public interface IAnimation
    {
        int HashCode { get; }
        Animator Animator { get; }
        void PlayAnimation();
        void StopAnimation();
    }
}