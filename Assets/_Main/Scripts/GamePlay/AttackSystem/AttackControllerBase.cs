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

        protected int SelectedAttackIndex = 0;

        public List<AttackBase> GetMeleeAttacks() => meleeAttacks;

        public abstract void SetSelectedMeleeAttack(StateMachine stateMachine);
        public abstract void SetSelectedRangedAttack(Type rangedAttackType, StateMachine stateMachine, IDamageable caster);
        public abstract void SetSelectedMeleeAttack(Type meleeAttackType, StateMachine stateMachine);
        public abstract void ScrollMeleeWeapon(float switchInput, StateMachine stateMachine);
        public abstract void SelectMeleeWeaponWithNo(int weaponNo, StateMachine stateMachine);
        public abstract AttackBase SelectAttackFromList(Type attackType, List<AttackBase> attacks);
    }
}