using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.StateMachineSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.BehaviourSystem
{
    [RequireComponent(typeof(StateMachine))]
    public abstract class BehaviourBase<T> : MonoBehaviour
where T : BehaviourBaseData
    {
        [SerializeField] protected T data;

        protected StateMachine stateMachine;
        protected InputBase _input;

        public T Data => data;

        protected virtual void Awake()
        {
            stateMachine = GetComponent<StateMachine>();
            _input = GetComponent<InputBase>();
        }
    } 
}