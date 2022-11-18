using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.HealthSystem;
using _Main.Scripts.GamePlay.StateMachineSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public abstract class AttackControllerBase : MonoBehaviour
    {
        [SerializeField] protected List<AttackBase> rangedAttacks = new List<AttackBase>();
        [SerializeField] protected List<AttackBase> meleeAttacks = new List<AttackBase>();

        public Action<AttackBase> OnSelectedMeleeAttackChanged;
        public Action<AttackBase> OnSelectedRangedAttackChanged;
        public RangedAttackBase SelectedRangedAttack { get; protected set; }
        public AttackBase SelectedMeleeAttack { get; protected set; }
        public MeleeAttackState meleeAttackState;
        public AimingState aimingState;

        protected int SelectedAttackIndex = 0;

        public List<AttackBase> GetMeleeAttacks() => meleeAttacks;
        public List<AttackBase> GetRangedAttacks() => rangedAttacks;

        public void SetMeleeAttackState(MeleeAttackState meleeState)
        {
            meleeAttackState = meleeState;
        }

        public void SetRangedAttackState(AimingState rangedState)
        {
            aimingState = rangedState;
        }

        public abstract void SetSelectedMeleeAttack();
        public abstract void SetSelectedRangedAttack(Type rangedAttackType, IDamageable caster);
        public abstract void SetSelectedMeleeAttack(Type meleeAttackType);
        public abstract void ScrollMeleeWeapon(float switchInput);
        public abstract void SelectMeleeWeaponWithNo(int weaponNo);
        public abstract AttackBase SelectAttackFromList(Type attackType, List<AttackBase> attacks);
    }
}