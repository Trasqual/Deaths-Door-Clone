using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.ActionSystem;
using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.AttackSystem.MeleeAttacks;
using _Main.Scripts.GamePlay.AttackSystem.RangedAttacks;
using _Main.Scripts.GamePlay.Indicators.AimingIndicator;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.StateMachine;
using _Main.Scripts.Others;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    [RequireComponent(typeof(PlayerMovementBase),
        typeof(PlayerAnimation))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData data = null;
        public InputBase Input { get; private set; }
        public CharacterController Controller { get; private set; }
        public PlayerAnimation PlayerAnim { get; private set; }

        [SerializeField] List<AttackBase> rangedAttacks = new List<AttackBase>();
        [SerializeField] List<AttackBase> meleeAttacks = new List<AttackBase>();

        private StateMachine.StateMachine stateMachine;
        
        private PlayerMovementBase _playerMovementBase = null;
        
        private AttackBase selectedRangedAttack;
        private AttackBase selectedMeleeAttack;

        private void Awake()
        {
            Input = GetComponent<InputBase>();
            Controller = GetComponent<CharacterController>();
            PlayerAnim = GetComponent<PlayerAnimation>();
            stateMachine = GetComponent<StateMachine.StateMachine>();
            _playerMovementBase = GetComponent<PlayerMovementBase>();
            stateMachine.Initialize(Input, _playerMovementBase, PlayerAnim.Animator);
        }

        private void Start()
        {
            GainMovementBehaviour();
            GainDodgeBehaviour();
            GainAimingBehaviour();
            stateMachine.SetInitialState(typeof(MovementState));
            
            SetSelectedMeleeAttack(typeof(UnarmedAttack));
            SetSelectedRangedAttack(typeof(BowAttack));
        }

        public void GainMovementBehaviour()
        {
            stateMachine.AddMovementState();
        }

        public void LoseMovementBehaviour()
        {
            stateMachine.RemoveState(typeof(MovementState));
        }

        public void GainDodgeBehaviour()
        {
            stateMachine.AddDodgeState(2F, .5F);
        }
        
        public void LoseDodgeBehaviour()
        {
            stateMachine.RemoveState(typeof(DodgeState));
        }
        
        public void GainAimingBehaviour()
        {
            stateMachine.AddAimingState(1F, .2F);
            var aimingBehaviour = stateMachine.GetState(typeof(AimingState));

            if (aimingBehaviour)
            {
                var targetGroupHandler = Instantiate(data.cameraTargetGroup, transform);
                var aimIndicator = Instantiate(data.aimingIndicator, transform);
                var aimBehaviourAction = aimingBehaviour.GetComponent<IAction>();
                targetGroupHandler.Init(aimBehaviourAction);
                aimIndicator.Init(aimBehaviourAction);
                
                targetGroupHandler.AddTarget(transform, .5F);
                targetGroupHandler.AddTarget(aimIndicator.transform, .75F);
            }
        }
        
        public void LoseAimingBehaviour()
        {
            stateMachine.RemoveState(typeof(AimingState));
            Destroy(GetComponentInChildren<CameraTargetGroup>().gameObject);
            Destroy(GetComponentInChildren<AimActionIndicator>().gameObject);
        }

        private void StartAiming()
        {
            stateMachine.ChangeState(typeof(AimingState));
        }

        private void PerformRoll()
        {
            stateMachine.ChangeState(typeof(DodgeState));
        }

        private void SetSelectedRangedAttack(Type rangedAttackType)
        {
            selectedRangedAttack = SelectAttackFromList(rangedAttackType, rangedAttacks);
            selectedRangedAttack.Init(stateMachine.GetState(typeof(AimingState)) as IAction);
        }

        private void SetSelectedMeleeAttack(Type meleeAttackType)
        {
            //selectedMeleeAttack = SelectAttackFromList(meleeAttackType, meleeAttacks);
            //selectedMeleeAttack.Init(stateMachine.AttackState);
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
            //stateMachine.ChangeState(stateMachine.AttackState);
        }

        private void OnEnable()
        {
            Input.OnAimActionStarted += StartAiming;
            Input.OnRollAction += PerformRoll;
            Input.OnAttackAction += Attack;
        }
    }
}