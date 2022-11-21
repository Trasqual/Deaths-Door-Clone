using _Main.Scripts.GamePlay.StateMachineSystem;

namespace _Main.Scripts.GamePlay.BehaviourSystem
{
    public class BossEnemyBehaviour : EnemyBehaviourBase
    {
        protected override void Start()
        {
            base.Start();

            GainAimingBehaviour();
            _attackController.SetSelectedRangedAttack();

            GainAttackBehaviour();
            _attackController.SetSelectedMeleeAttack();

        }

        public void GainAttackBehaviour()
        {
            stateMachine.AddAttackState(1f, _attackController);
            _attackController.SetMeleeAttackState(stateMachine.GetState(typeof(MeleeAttackState)) as MeleeAttackState);
        }

        public void GainAimingBehaviour()
        {
            stateMachine.AddAimingState(1f, .5f, _attackController);
            _attackController.SetRangedAttackState(stateMachine.GetState(typeof(AimingState)) as AimingState, _healthManager);
        }

        private void Attack()
        {
            stateMachine.ChangeState(typeof(MeleeAttackState));
        }

        private void StartAiming()
        {
            stateMachine.ChangeState(typeof(AimingState));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _input.OnAttackActionStarted += Attack;
            _input.OnAimActionStarted += StartAiming;

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _input.OnAttackActionStarted -= Attack;
            _input.OnAimActionStarted -= StartAiming;

        }
    }
}