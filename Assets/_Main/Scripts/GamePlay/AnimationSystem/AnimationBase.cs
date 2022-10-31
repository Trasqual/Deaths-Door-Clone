using UnityEngine;

namespace _Main.Scripts.GamePlay.AnimationSystem
{
    public abstract class AnimationBase : MonoBehaviour
    {
        [SerializeField] protected Animator animator = null;
        public Animator Animator => animator;
    }
}