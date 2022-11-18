using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.StateMachineSystem;

namespace _Main.Scripts.GamePlay.BehaviourSystem
{
    public class RangedEnemyBehaviour : EnemyBehaviourBase
    {
        protected override void Start()
        {
            base.Start();
            GainAimingBehaviour();
            _attackController.SetSelectedRangedAttack(typeof(RangedEnemyAttack), _healthManager);
        }

        public void GainAimingBehaviour()
        {
            stateMachine.AddAimingState(1f, .5f, _attackController);
            _attackController.SetRangedAttackState(stateMachine.GetState(typeof(AimingState)) as AimingState);
        }

        private void StartAiming()
        {
            stateMachine.ChangeState(typeof(AimingState));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _input.OnAimActionStarted += StartAiming;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _input.OnAimActionStarted -= StartAiming;
        }
    }
}