using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class BehaviourBase : MonoBehaviour
{
    [SerializeField] protected BehaviourData data;
    
    protected StateMachine stateMachine;
    protected InputBase _input;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        _input = GetComponent<InputBase>();
    }
}