using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.ActionSystem;
using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.AttackSystem.RangedAttacks;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

public class AttackController : AttackControllerBase
{
    public override void SwitchMeleeWeapon(float switchInput, StateMachine stateMachine)
    {
        if (stateMachine.CurrentState is MeleeAttackState) return;
        SelectedAttackIndex += (int)Mathf.Sign(switchInput);
        if (SelectedAttackIndex > meleeAttacks.Count - 1)
        {
            SelectedAttackIndex = 0;
        }
        if (SelectedAttackIndex < 0)
        {
            SelectedAttackIndex = meleeAttacks.Count - 1;
        }
        SetSelectedMeleeAttack(stateMachine);
    }
    
    public override void SetSelectedMeleeAttack(StateMachine stateMachine)
    {
        if(SelectedMeleeAttack != null)
        {
            SelectedMeleeAttack.Release(stateMachine.GetState(typeof(MeleeAttackState)) as IAction);
        }
        SelectedMeleeAttack = meleeAttacks[SelectedAttackIndex];
        SelectedMeleeAttack.Init(stateMachine.GetState(typeof(MeleeAttackState)) as IAction);
        OnSelectedMeleeAttackChanged?.Invoke(SelectedMeleeAttack);
    }

    public override void SetSelectedRangedAttack(Type rangedAttackType, StateMachine stateMachine, IDamageable caster)
    {
        SelectedRangedAttack = (RangedAttackBase)SelectAttackFromList(rangedAttackType, rangedAttacks);
        SelectedRangedAttack.Init(stateMachine.GetState(typeof(AimingState)) as IAction, caster);
        OnSelectedRangedAttackChanged?.Invoke(SelectedRangedAttack);
    }

    public override void SetSelectedMeleeAttack(Type meleeAttackType, StateMachine stateMachine)
    {
        SelectedMeleeAttack = SelectAttackFromList(meleeAttackType, meleeAttacks);
        SelectedMeleeAttack.Init(stateMachine.GetState(typeof(MeleeAttackState)) as IAction);
        OnSelectedMeleeAttackChanged?.Invoke(SelectedMeleeAttack);
    }
    
    public override AttackBase SelectAttackFromList(Type attackType, List<AttackBase> attacks)
    {
        foreach (var attack in attacks)
        {
            if (attack.GetType() == attackType)
            {
                return attack;
            }
        }
        return null;
    }
}