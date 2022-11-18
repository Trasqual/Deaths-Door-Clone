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

        private void SelectNextAttack(int switchInput)
        {
            if (switchInput != _attackController.GetMeleeAttacks().IndexOf(_attackController.SelectedMeleeAttack))
            {
                _attackController.SelectMeleeWeaponWithNo(switchInput);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _input.OnAttackActionStarted += Attack;
            _input.OnMeleeWeaponSwitchedWithID += SelectNextAttack;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _input.OnAttackActionStarted -= Attack;
            _input.OnMeleeWeaponSwitchedWithID -= SelectNextAttack;
        }
    }
}