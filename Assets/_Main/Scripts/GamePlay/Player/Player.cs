using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.ActionSystem;
using _Main.Scripts.GamePlay.AttackSystem;
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
    public class Player : BehaviourBase
    {
        [SerializeField] private PlayerData data = null;
        public InputBase Input { get; private set; }
        public CharacterController Controller { get; private set; }
        public PlayerAnimation PlayerAnim { get; private set; }

        [SerializeField] List<AttackBase> rangedAttacks = new List<AttackBase>();
        [SerializeField] List<AttackBase> meleeAttacks = new List<AttackBase>();

        private PlayerMovementBase _playerMovementBase = null;
        private HealthComponentBase _playerHealthManager = null;

        int selectedAttackIndex;

        protected override void Awake()
        {
            base.Awake();
            Input = GetComponent<InputBase>();
            Controller = GetComponent<CharacterController>();
            PlayerAnim = GetComponent<PlayerAnimation>();
            _playerMovementBase = GetComponent<PlayerMovementBase>();
            _playerHealthManager = GetComponent<HealthComponentBase>();
            stateMachine.Initialize(Input, _playerMovementBase, PlayerAnim.Animator, _playerHealthManager);
        }

        private void Start()
        {
            GainMovementBehaviour();
            GainDodgeBehaviour();
            GainAttackingBehaviour();
            GainAimingBehaviour();
            GainDamageTakingBehaviour();
            GainDeathBehaviour();
            stateMachine.SetInitialState(typeof(MovementState));

            SetSelectedMeleeAttack();
            //SetSelectedMeleeAttack(typeof(UnarmedAttack));
            SetSelectedRangedAttack(typeof(BowAttack));
        }

        #region Behaviours
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
            stateMachine.AddAttackState(this);
        }

        public void LoseAttackBehaviour()
        {
            stateMachine.RemoveState(typeof(AttackState));
        }

        public void GainAimingBehaviour()
        {
            stateMachine.AddAimingState(10F, .2F, this);
            var aimingBehaviour = stateMachine.GetState(typeof(AimingState));

            if (aimingBehaviour)
            {
                var targetGroupHandler = Instantiate(data.cameraTargetGroup, transform);
                var aimIndicator = Instantiate(data.aimingIndicator, transform);
                var aimBehaviourAction = (IAction)aimingBehaviour;
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
        #endregion

        #region Actions(Melee/Ranged/Roll)
        private void Attack()
        {
            stateMachine.ChangeState(typeof(AttackState));
        }

        private void StartAiming()
        {
            stateMachine.ChangeState(typeof(AimingState));
        }

        private void PerformRoll()
        {
            stateMachine.ChangeState(typeof(DodgeState));
        }
        #endregion

        #region WeaponSelection(Melee/Ranged)

        private void SwitchMeleeWeapon(float switchInput)
        {
            if (stateMachine.CurrentState is AttackState) return;
            selectedAttackIndex += (int)Mathf.Sign(switchInput);
            if (selectedAttackIndex > meleeAttacks.Count - 1)
            {
                selectedAttackIndex = 0;
            }
            if (selectedAttackIndex < 0)
            {
                selectedAttackIndex = meleeAttacks.Count - 1;
            }
            SetSelectedMeleeAttack();
        }

        private void SetSelectedMeleeAttack()
        {
            if(SelectedMeleeAttack != null)
            {
                SelectedMeleeAttack.Release(stateMachine.GetState(typeof(AttackState)) as IAction);
            }
            SelectedMeleeAttack = meleeAttacks[selectedAttackIndex];
            SelectedMeleeAttack.Init(stateMachine.GetState(typeof(AttackState)) as IAction);
            OnSelectedMeleeAttackChanged?.Invoke(SelectedMeleeAttack);
        }

        private void SetSelectedRangedAttack(Type rangedAttackType)
        {
            SelectedRangedAttack = SelectAttackFromList(rangedAttackType, rangedAttacks);
            SelectedRangedAttack.Init(stateMachine.GetState(typeof(AimingState)) as IAction);
            OnSelectedRangedAttackChanged?.Invoke(SelectedRangedAttack);
        }

        private void SetSelectedMeleeAttack(Type meleeAttackType)
        {
            SelectedMeleeAttack = SelectAttackFromList(meleeAttackType, meleeAttacks);
            SelectedMeleeAttack.Init(stateMachine.GetState(typeof(AttackState)) as IAction);
            OnSelectedMeleeAttackChanged?.Invoke(SelectedMeleeAttack);
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
        #endregion

        #region Health(TakeDamage/Death)
        protected void TakeDamage(int i)
        {
            stateMachine.ChangeState(typeof(DamageTakenState));
        }

        protected void Die()
        {
            Controller.enabled = false;
            stateMachine.ChangeState(typeof(DeathState));
        }
        #endregion

        private void OnEnable()
        {
            Input.OnAimActionStarted += StartAiming;
            Input.OnRollAction += PerformRoll;
            Input.OnAttackActionStarted += Attack;
            Input.OnMeleeWeaponSwitched += SwitchMeleeWeapon;

            _playerHealthManager.OnDamageTaken += TakeDamage;
            _playerHealthManager.OnDeath += Die;
        }

        private void OnDisable()
        {
            Input.OnAimActionStarted -= StartAiming;
            Input.OnRollAction -= PerformRoll;
            Input.OnAttackActionStarted -= Attack;

            _playerHealthManager.OnDamageTaken -= TakeDamage;
            _playerHealthManager.OnDeath -= Die;
        }
    }
}