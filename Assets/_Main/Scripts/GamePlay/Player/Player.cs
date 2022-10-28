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
            stateMachine.Initialize(_input, _playerMovement, PlayerAnim.Animator);
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

            _attackController.SetSelectedMeleeAttack(stateMachine);
            _attackController.SetSelectedRangedAttack(typeof(BowAttack), stateMachine);
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
            stateMachine.AddAttackState(_attackController);
        }

        public void LoseAttackBehaviour()
        {
            stateMachine.RemoveState(typeof(MeleeAttackState));
        }

        public void GainAimingBehaviour()
        {
            stateMachine.AddAimingState(10F, .2F, _attackController);
            var aimingBehaviour = stateMachine.GetState(typeof(AimingState));

            if (aimingBehaviour)
            {
                var playerData = (PlayerBehaviourData) data;
                var targetGroupHandler = Instantiate(playerData.cameraTargetGroup, transform);
                var aimIndicator = Instantiate(playerData.aimingIndicator, transform);
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
            stateMachine.AddDamageTakenState(data.DamageTakenDuration);
        }

        public void GainDeathBehaviour()
        {
            stateMachine.AddDeathState();
        }
        #endregion

        #region Actions(Melee/Ranged/Roll)
        private void Attack()
        {
            stateMachine.ChangeState(typeof(MeleeAttackState));
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
            _attackController.SwitchMeleeWeapon(switchInput, stateMachine);
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