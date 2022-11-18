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
        public AttackBase SelectedRangedAttack { get; protected set; }
        public AttackBase SelectedMeleeAttack { get; protected set; }
        protected MeleeAttackState meleeAttackState;
        protected AimingState aimingState;
        protected IDamageable _caster;

        protected int SelectedMeleeAttackIndex = 0;
        protected int SelectedRangedAttackIndex = 0;

        public List<AttackBase> GetMeleeAttacks() => meleeAttacks;
        public List<AttackBase> GetRangedAttacks() => rangedAttacks;

        public void SetMeleeAttackState(MeleeAttackState meleeState)
        {
            meleeAttackState = meleeState;
        }

        public void SetRangedAttackState(AimingState rangedState, IDamageable caster)
        {
            aimingState = rangedState;
            _caster = caster;
        }

        public abstract void ScrollMeleeWeapon(float switchInput);
        public abstract void SetSelectedMeleeAttack();
        public abstract void SetSelectedMeleeAttack(int weaponNo);

        public abstract void ScrollRangedWeapon(float switchInput);
        public abstract void SetSelectedRangedAttack();
        public abstract void SetSelectedRangedAttack(int weaponNo);
    }
}