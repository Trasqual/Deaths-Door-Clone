using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class BehaviourBase : MonoBehaviour
{
    [SerializeField] protected BehaviourData _data;
    
    protected StateMachine _stateMachine;
    protected InputBase _input;

    protected virtual void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _input = GetComponent<InputBase>();
    }
}