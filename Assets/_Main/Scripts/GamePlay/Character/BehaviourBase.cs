using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class BehaviourBase : MonoBehaviour
{
    protected StateMachine stateMachine;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }
}