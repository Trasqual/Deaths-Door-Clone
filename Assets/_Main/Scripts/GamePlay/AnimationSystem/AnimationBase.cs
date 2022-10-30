using UnityEngine;

namespace _Main.Scripts.GamePlay.AnimationSystem
{
    public abstract class AnimationBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator = null;
        public Animator Animator => _animator;
    }
}