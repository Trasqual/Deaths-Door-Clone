using _Main.Scripts.GamePlay.StateMachineSystem;

namespace _Main.Scripts.GamePlay.BehaviourSystem
{
    public class MeleeEnemyBehaviour : EnemyBehaviourBase
    {
        protected override void Start()
        {
            base.Start();
            GainAttackBehaviour();
            _attackController.SetSelectedMeleeAttack();
        }

        public void GainAttackBehaviour()
        {
            stateMachine.AddAttackState(_attackController);
            _attackController.SetMeleeAttackState(stateMachine.GetState(typeof(MeleeAttackState)) as MeleeAttackState);
        }

        private void Attack()
        {
            stateMachine.ChangeState(typeof(MeleeAttackState));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _input.OnAttackActionStarted += Attack;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _input.OnAttackActionStarted -= Attack;
        }
    }
}