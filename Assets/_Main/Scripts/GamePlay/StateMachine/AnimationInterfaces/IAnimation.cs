using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public interface IAnimation
    {
        public int HashCode { get; }
        public Animator Animator { get; }
        public void PlayAnimation();
        public void StopAnimation();
    }
}