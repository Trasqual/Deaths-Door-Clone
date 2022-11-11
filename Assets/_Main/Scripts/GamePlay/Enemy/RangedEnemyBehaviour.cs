using _Main.Scripts.GamePlay.StateMachine;

public class RangedEnemyBehaviour : EnemyBehaviourBase
{
    protected override void Start()
    {
        base.Start();
        GainAimingBehaviour();
        AttackController.SetSelectedRangedAttack(typeof(RangedEnemyAttack), stateMachine, _healthManager);
        _agent.stoppingDistance = AttackController.SelectedRangedAttack.CurrentComboDamageData.attackRange;
    }

    public void GainAimingBehaviour()
    {
        stateMachine.AddAimingState(10f, .5f, AttackController);
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
