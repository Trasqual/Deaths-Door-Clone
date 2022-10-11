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
    public class Player : CharacterBase
    {
        [SerializeField] private PlayerData data = null;
        public InputBase Input { get; private set; }
        public CharacterController Controller { get; private set; }
        public PlayerAnimation PlayerAnim { get; private set; }

        [SerializeField] List<AttackBase> rangedAttacks = new List<AttackBase>();
        [SerializeField] List<AttackBase> meleeAttacks = new List<AttackBase>();

        private PlayerMovementBase _playerMovementBase = null;
        private HealthManagerBase _playerHealthManager = null;

        private AttackBase selectedRangedAttack;
        private AttackBase selectedMeleeAttack;

        protected override void Awake()
        {
            base.Awake();
            Input = GetComponent<InputBase>();
            Controller = GetComponent<CharacterController>();
            PlayerAnim = GetComponent<PlayerAnimation>();
            _playerMovementBase = GetComponent<PlayerMovementBase>();
            _playerHealthManager = GetComponent<HealthManagerBase>();
            stateMachine.Initialize(Input, _playerMovementBase, PlayerAnim.Animator, _playerHealthManager);
        }

        private void Start()
        {
            GainMovementBehaviour();
            GainDodgeBehaviour();
            GainAimingBehaviour();
            GainDamageTakingBehaviour();
            GainDeathBehaviour();
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

        public void GainAttackingBehaviour()
        {
            stateMachine.AddAttackState();
        }

        public void LoseAttackBehaviour()
        {
            stateMachine.RemoveState(typeof(AttackState));
        }

        public void GainAimingBehaviour()
        {
            stateMachine.AddAimingState(10F, .2F);
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

        public void GainDamageTakingBehaviour()
        {
            stateMachine.AddDamageTakenState(data.damageTakenDuration);
        }

        public void GainDeathBehaviour()
        {
            stateMachine.AddDeathState();
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
            selectedMeleeAttack = SelectAttackFromList(meleeAttackType, meleeAttacks);
            selectedMeleeAttack.Init(stateMachine.GetState(typeof(AttackState)) as IAction);
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
            stateMachine.ChangeState(typeof(AttackState));
        }

        protected override void TakeDamage(int i)
        {
            stateMachine.ChangeState(typeof(DamageTakenState));
        }

        protected override void Die()
        {
            Controller.enabled = false;
            stateMachine.ChangeState(typeof(DeathState));
        }

        private void OnEnable()
        {
            Input.OnAimActionStarted += StartAiming;
            Input.OnRollAction += PerformRoll;
            Input.OnAttackAction += Attack;

            _playerHealthManager.OnDamageTaken += TakeDamage;
            _playerHealthManager.OnDeath += Die;
        }

        private void OnDisable()
        {
            Input.OnAimActionStarted -= StartAiming;
            Input.OnRollAction -= PerformRoll;
            Input.OnAttackAction -= Attack;

            _playerHealthManager.OnDamageTaken -= TakeDamage;
            _playerHealthManager.OnDeath -= Die;
        }
    }
}