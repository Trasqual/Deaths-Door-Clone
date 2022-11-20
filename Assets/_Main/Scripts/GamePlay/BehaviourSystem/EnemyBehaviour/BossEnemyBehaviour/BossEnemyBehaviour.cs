using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.StateMachineSystem;

namespace _Main.Scripts.GamePlay.BehaviourSystem
{
    public class BossEnemyBehaviour : EnemyBehaviourBase
    {
        protected override void Start()
        {
            base.Start();

            GainAttackBehaviour();
            _attackController.SetSelectedMeleeAttack();

            GainAimingBehaviour();
        }

        public void GainAttackBehaviour()
        {
            stateMachine.AddAttackState(1f, _attackController);
            _attackController.SetMeleeAttackState(stateMachine.GetState(typeof(MeleeAttackState)) as MeleeAttackState);
        }

        public void GainAimingBehaviour()
        {
            stateMachine.AddAimingState(10f, .5f, _attackController);
        }

        private void Attack()
        {
            stateMachine.ChangeState(typeof(MeleeAttackState));
        }

        private void SelectNextAttack(int switchInput)
        {
            if (switchInput != _attackController.GetMeleeAttacks().IndexOf(_attackController.SelectedMeleeAttack))
            {
                _attackController.SetSelectedMeleeAttack(switchInput);
            }
        }

        private void StartAiming()
        {
            stateMachine.ChangeState(typeof(AimingState));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _input.OnAttackActionStarted += Attack;
            _input.OnMeleeWeaponSwitchedWithID += SelectNextAttack;
            _input.OnAimActionStarted += StartAiming;

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _input.OnAttackActionStarted -= Attack;
            _input.OnMeleeWeaponSwitchedWithID -= SelectNextAttack;
            _input.OnAimActionStarted -= StartAiming;

        }
    }
}