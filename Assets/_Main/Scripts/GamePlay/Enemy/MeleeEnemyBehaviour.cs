using _Main.Scripts.GamePlay.StateMachine;

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
