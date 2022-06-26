using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.AttackSystem.MeleeAttacks;
using _Main.Scripts.GamePlay.AttackSystem.RangedAttacks;
using _Main.Scripts.GamePlay.Indicators.AimingIndicator;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.Others;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    [RequireComponent(typeof(PlayerMovement),
        typeof(PlayerAnimation))]
    public class Player : MonoBehaviour
    {
        public PlayerData Data { get; private set; }
        public InputBase Input { get; private set; }
        public CharacterController Controller { get; private set; }
        public PlayerAnimation PlayerAnim { get; private set; }

        [SerializeField] List<AttackBase> rangedAttacks = new List<AttackBase>();
        [SerializeField] List<AttackBase> meleeAttacks = new List<AttackBase>();

        private StateMachine.StateMachine stateMachine;
        private CinemachineTargetGroupHandler targetGroupHandler;
        private AimActionIndicator aimIndicatior;

        private AttackBase selectedRangedAttack;
        private AttackBase selectedMeleeAttack;

        private void Awake()
        {
            //Data = GetComponent<PlayerData>();
            Input = GetComponent<InputBase>();
            Controller = GetComponent<CharacterController>();
            PlayerAnim = GetComponent<PlayerAnimation>();
            stateMachine = GetComponent<StateMachine.StateMachine>();
            targetGroupHandler = GetComponentInChildren<CinemachineTargetGroupHandler>();
            aimIndicatior = GetComponentInChildren<AimActionIndicator>();

            Input.OnAimActionStarted += StartAiming;
            Input.OnAimActionEnded += EndAiming;
            Input.OnRollAction += PerformRoll;
            Input.OnAttackAction += Attack;
        }

        private void Start()
        {
            SetSelectedMeleeAttack(typeof(UnarmedAttack));
            SetSelectedRangedAttack(typeof(BowAttack));
            targetGroupHandler.Init(stateMachine.AimingState);
            aimIndicatior.Init(stateMachine.AimingState);
        }

        private void StartAiming()
        {
            stateMachine.ChangeState(stateMachine.AimingState);
        }

        private void EndAiming()
        {
            stateMachine.AimingState.EndAim();
        }

        private void PerformRoll()
        {
            stateMachine.ChangeState(stateMachine.DodgeState);
        }

        private void SetSelectedRangedAttack(Type rangedAttackType)
        {
            selectedRangedAttack = SelectAttackFromList(rangedAttackType, rangedAttacks);
            selectedRangedAttack.Init(stateMachine.AimingState);
        }

        private void SetSelectedMeleeAttack(Type meleeAttackType)
        {
            selectedMeleeAttack = SelectAttackFromList(meleeAttackType, meleeAttacks);
            selectedMeleeAttack.Init(stateMachine.AttackState);
        }

        private AttackBase SelectAttackFromList(Type attackType, List<AttackBase> attacks)
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

        private void Attack()
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
    }
}