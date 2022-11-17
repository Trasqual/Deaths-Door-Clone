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
            AttackController.SetSelectedMeleeAttack(stateMachine);

            GainAimingBehaviour();
            AttackController.SetSelectedRangedAttack(typeof(RangedEnemyAttack), stateMachine, _healthManager);
        }

        public void GainAttackBehaviour()
        {
            stateMachine.AddAttackState(AttackController);
        }

        public void GainAimingBehaviour()
        {
            stateMachine.AddAimingState(10f, .5f, AttackController);
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