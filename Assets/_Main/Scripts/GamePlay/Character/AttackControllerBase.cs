using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

public abstract class AttackControllerBase : MonoBehaviour
{
    [SerializeField] protected List<AttackBase> rangedAttacks = new List<AttackBase>();
    [SerializeField] protected List<AttackBase> meleeAttacks = new List<AttackBase>();
    
    public Action<AttackBase> OnSelectedMeleeAttackChanged;
    public Action<AttackBase> OnSelectedRangedAttackChanged;
    public AttackBase SelectedRangedAttack { get; protected set; }
    public AttackBase SelectedMeleeAttack { get; protected set; }

    protected int SelectedAttackIndex = 0;
    
    public abstract void SetSelectedMeleeAttack(StateMachine stateMachine);
    public abstract void SetSelectedRangedAttack(Type rangedAttackType, StateMachine stateMachine);
    public abstract void SetSelectedMeleeAttack(Type meleeAttackType, StateMachine stateMachine);
    public abstract void SwitchMeleeWeapon(float switchInput, StateMachine stateMachine);
    public abstract AttackBase SelectAttackFromList(Type attackType, List<AttackBase> attacks);
}