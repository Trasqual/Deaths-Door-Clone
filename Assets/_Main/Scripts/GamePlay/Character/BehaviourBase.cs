using System;
using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class BehaviourBase : MonoBehaviour
{
    protected StateMachine stateMachine;
    
    public Action<AttackBase> OnSelectedMeleeAttackChanged;
    public Action<AttackBase> OnSelectedRangedAttackChanged;
    public AttackBase SelectedRangedAttack { get; protected set; }
    public AttackBase SelectedMeleeAttack { get; protected set; }

    protected virtual void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }
}