using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class AttackController : AttackControllerBase
    {
        #region MeleeAttackSelection

        public override void ScrollMeleeWeapon(float switchInput)
        {
            SelectedMeleeAttackIndex += (int)Mathf.Sign(switchInput);
            if (SelectedMeleeAttackIndex > meleeAttacks.Count - 1)
            {
                SelectedMeleeAttackIndex = 0;
            }
            if (SelectedMeleeAttackIndex < 0)
            {
                SelectedMeleeAttackIndex = meleeAttacks.Count - 1;
            }
            SetSelectedMeleeAttack();
        }

        public override void SetSelectedMeleeAttack(int weaponNo)
        {
            if (SelectedMeleeAttackIndex == weaponNo) return;
            SelectedMeleeAttackIndex = weaponNo;
            SetSelectedMeleeAttack();
        }

        public override void SetSelectedMeleeAttack()
        {
            if (SelectedMeleeAttack != null)
            {
                SelectedMeleeAttack.Release(meleeAttackState);
            }
            SelectedMeleeAttack = meleeAttacks[SelectedMeleeAttackIndex];
            SelectedMeleeAttack.Init(meleeAttackState);
            OnSelectedMeleeAttackChanged?.Invoke(SelectedMeleeAttack);
        }
        #endregion

        #region RangedAttackSelection
        public override void ScrollRangedWeapon(float switchInput)
        {
            SelectedRangedAttackIndex += (int)Mathf.Sign(switchInput);
            if (SelectedRangedAttackIndex > rangedAttacks.Count - 1)
            {
                SelectedRangedAttackIndex = 0;
            }
            if (SelectedRangedAttackIndex < 0)
            {
                SelectedRangedAttackIndex = rangedAttacks.Count - 1;
            }
            SetSelectedRangedAttack();
        }

        public override void SetSelectedRangedAttack(int weaponNo)
        {
            if (SelectedRangedAttackIndex == weaponNo) return;
            SelectedRangedAttackIndex = weaponNo;
            SetSelectedRangedAttack();
        }

        public override void SetSelectedRangedAttack()
        {
            if (SelectedRangedAttack != null)
            {
                SelectedRangedAttack.Release(aimingState);
            }
            SelectedRangedAttack = rangedAttacks[SelectedRangedAttackIndex];
            var rangedAttack = (RangedAttackBase)SelectedRangedAttack;
            rangedAttack.Init(aimingState, _caster);
            OnSelectedRangedAttackChanged?.Invoke(SelectedMeleeAttack);
        }
        #endregion
    }
}