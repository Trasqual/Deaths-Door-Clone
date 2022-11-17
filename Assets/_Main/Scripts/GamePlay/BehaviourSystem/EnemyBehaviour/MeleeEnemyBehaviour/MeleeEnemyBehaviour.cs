using _Main.Scripts.GamePlay.StateMachineSystem;

namespace _Main.Scripts.GamePlay.BehaviourSystem
{
    public class MeleeEnemyBehaviour : EnemyBehaviourBase
    {
        protected override void Start()
        {
            base.Start();
            GainAttackBehaviour();
            AttackController.SetSelectedMeleeAttack(stateMachine);
        }

        public void GainAttackBehaviour()
        {
            stateMachine.AddAttackState(AttackController);
        }

        private void Attack()
        {
            stateMachine.ChangeState(typeof(MeleeAttackState));
        }

        private void SelectNextAttack(int switchInput)
        {
            if (switchInput != AttackController.GetMeleeAttacks().IndexOf(AttackController.SelectedMeleeAttack))
            {
                AttackController.SelectMeleeWeaponWithNo(switchInput, stateMachine);
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