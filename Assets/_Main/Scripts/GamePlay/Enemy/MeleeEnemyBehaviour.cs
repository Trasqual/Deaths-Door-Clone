using _Main.Scripts.GamePlay.StateMachine;

public class MeleeEnemyBehaviour : EnemyBehaviourBase
{
    protected override void Start()
    {
        base.Start();
        GainAttackBehaviour();
        AttackController.SetSelectedMeleeAttack(stateMachine);
        _agent.stoppingDistance = AttackController.SelectedMeleeAttack.CurrentComboDamageData.attackRange;
    }

    public void GainAttackBehaviour()
    {
        stateMachine.AddAttackState(AttackController);
    }

    private void Attack()
    {
        stateMachine.ChangeState(typeof(MeleeAttackState));
    }

    private void SwitchMeleeWeapon(float switchInput)
    {
        AttackController.SwitchMeleeWeapon(switchInput, stateMachine);
        _agent.stoppingDistance = AttackController.SelectedMeleeAttack.CurrentComboDamageData.attackRange;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _input.OnAttackActionStarted += Attack;
        _input.OnMeleeWeaponSwitched += SwitchMeleeWeapon;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _input.OnAttackActionStarted -= Attack;
        _input.OnMeleeWeaponSwitched -= SwitchMeleeWeapon;
    }
}
