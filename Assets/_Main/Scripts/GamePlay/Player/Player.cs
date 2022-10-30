using _Main.Scripts.GamePlay.ActionSystem;
using _Main.Scripts.GamePlay.AttackSystem.RangedAttacks;
using _Main.Scripts.GamePlay.Indicators.AimingIndicator;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.StateMachine;
using _Main.Scripts.Others;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    [RequireComponent(typeof(PlayerMovementBase),
        typeof(PlayerAnimation), 
        typeof(HealthComponentBase))]
    public class Player : BehaviourBase
    {
        public CharacterController Controller { get; private set; }
        public PlayerAnimation PlayerAnim { get; private set; }

        private PlayerMovementBase _playerMovement = null;
        private HealthComponentBase _playerHealthManager = null;
        private AttackControllerBase _attackController = null;
        
        protected override void Awake()
        {
            base.Awake();
            Controller = GetComponent<CharacterController>();
            PlayerAnim = GetComponent<PlayerAnimation>();
            _playerMovement = GetComponent<PlayerMovementBase>();
            _playerHealthManager = GetComponent<HealthComponentBase>();
            _attackController = GetComponent<AttackControllerBase>();
            _stateMachine.Initialize(_input, _playerMovement, PlayerAnim.Animator);
        }

        private void Start()
        {
            GainMovementBehaviour();
            GainDodgeBehaviour();
            GainAttackingBehaviour();
            GainAimingBehaviour();
            GainDamageTakingBehaviour();
            GainDeathBehaviour();
            _stateMachine.SetInitialState(typeof(MovementState));

            _attackController.SetSelectedMeleeAttack(_stateMachine);
            _attackController.SetSelectedRangedAttack(typeof(RangedAttackBase), _stateMachine, _playerHealthManager);
        }

        #region Behaviours
        public void GainMovementBehaviour()
        {
            _stateMachine.AddMovementState();
        }

        public void LoseMovementBehaviour()
        {
            _stateMachine.RemoveState(typeof(MovementState));
        }

        public void GainDodgeBehaviour()
        {
            _stateMachine.AddDodgeState(2F, .5F);
        }

        public void LoseDodgeBehaviour()
        {
            _stateMachine.RemoveState(typeof(DodgeState));
        }

        public void GainAttackingBehaviour()
        {
            _stateMachine.AddAttackState(_attackController);
        }

        public void LoseAttackBehaviour()
        {
            _stateMachine.RemoveState(typeof(MeleeAttackState));
        }

        public void GainAimingBehaviour()
        {
            _stateMachine.AddAimingState(10F, .2F, _attackController);
            var aimingBehaviour = _stateMachine.GetState(typeof(AimingState));

            if (aimingBehaviour)
            {
                var playerData = (PlayerBehaviourData) _data;
                var targetGroupHandler = Instantiate(playerData.CameraTargetGroup, transform);
                var aimIndicator = Instantiate(playerData.AimingIndicator, transform);
                var aimBehaviourAction = (IAction)aimingBehaviour;
                targetGroupHandler.Init(aimBehaviourAction);
                aimIndicator.Init(aimBehaviourAction);

                targetGroupHandler.AddTarget(transform, .5F);
                targetGroupHandler.AddTarget(aimIndicator.transform, .75F);
            }
        }

        public void LoseAimingBehaviour()
        {
            _stateMachine.RemoveState(typeof(AimingState));
            Destroy(GetComponentInChildren<CameraTargetGroup>().gameObject);
            Destroy(GetComponentInChildren<AimActionIndicator>().gameObject);
        }

        public void GainDamageTakingBehaviour()
        {
            _stateMachine.AddDamageTakenState(_data.DamageTakenDuration);
        }

        public void GainDeathBehaviour()
        {
            _stateMachine.AddDeathState();
        }
        #endregion

        #region Actions(Melee/Ranged/Roll)
        private void Attack()
        {
            _stateMachine.ChangeState(typeof(MeleeAttackState));
        }

        private void StartAiming()
        {
            _stateMachine.ChangeState(typeof(AimingState));
        }

        private void PerformRoll()
        {
            _stateMachine.ChangeState(typeof(DodgeState));
        }
        #endregion

        #region WeaponSelection(Melee/Ranged)
        
        private void SwitchMeleeWeapon(float switchInput)
        {
            _attackController.SwitchMeleeWeapon(switchInput, _stateMachine);
        }

        #endregion

        #region Health(TakeDamage/Death)
        protected void TakeDamage(int i)
        {
            _stateMachine.ChangeState(typeof(DamageTakenState));
        }

        protected void Die()
        {
            Controller.enabled = false;
            _stateMachine.ChangeState(typeof(DeathState));
        }
        #endregion

        private void OnEnable()
        {
            _input.OnAimActionStarted += StartAiming;
            _input.OnRollAction += PerformRoll;
            _input.OnAttackActionStarted += Attack;
            _input.OnMeleeWeaponSwitched += SwitchMeleeWeapon;

            _playerHealthManager.OnDamageTaken += TakeDamage;
            _playerHealthManager.OnDeath += Die;
        }

        private void OnDisable()
        {
            _input.OnAimActionStarted -= StartAiming;
            _input.OnRollAction -= PerformRoll;
            _input.OnAttackActionStarted -= Attack;

            _playerHealthManager.OnDamageTaken -= TakeDamage;
            _playerHealthManager.OnDeath -= Die;
        }
    }
}