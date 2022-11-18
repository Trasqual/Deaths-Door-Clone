using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class AttackController : AttackControllerBase
    {
        public override void ScrollMeleeWeapon(float switchInput)
        {
            SelectedAttackIndex += (int)Mathf.Sign(switchInput);
            if (SelectedAttackIndex > meleeAttacks.Count - 1)
            {
                SelectedAttackIndex = 0;
            }
            if (SelectedAttackIndex < 0)
            {
                SelectedAttackIndex = meleeAttacks.Count - 1;
            }
            SetSelectedMeleeAttack();
        }

        public override void SelectMeleeWeaponWithNo(int weaponNo)
        {
            SelectedAttackIndex = weaponNo;
            SetSelectedMeleeAttack();
        }

        public override void SetSelectedMeleeAttack()
        {
            if (SelectedMeleeAttack != null)
            {
                SelectedMeleeAttack.Release(meleeAttackState);
            }
            SelectedMeleeAttack = meleeAttacks[SelectedAttackIndex];
            SelectedMeleeAttack.Init(meleeAttackState);
            OnSelectedMeleeAttackChanged?.Invoke(SelectedMeleeAttack);
        }

        public override void SetSelectedRangedAttack(Type rangedAttackType, IDamageable caster)
        {
            SelectedRangedAttack = (RangedAttackBase)SelectAttackFromList(rangedAttackType, rangedAttacks);
            SelectedRangedAttack.Init(aimingState, caster);
            OnSelectedRangedAttackChanged?.Invoke(SelectedRangedAttack);
        }

        public override void SetSelectedMeleeAttack(Type meleeAttackType)
        {
            SelectedMeleeAttack = SelectAttackFromList(meleeAttackType, meleeAttacks);
            SelectedMeleeAttack.Init(meleeAttackState);
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
}