using _Main.Scripts.GamePlay.MovementSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : BehaviourBase
{
    private NavMeshAgent _agent;
    private HealthComponentBase _healthManager;
    private MovementBase _movementBase;
    private Animator _anim;

    public AttackControllerBase AttackController { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _healthManager = GetComponent<HealthComponentBase>();
        _agent = GetComponent<NavMeshAgent>();
        _movementBase = GetComponent<MovementBase>();
        _anim = GetComponentInChildren<Animator>();
        AttackController = GetComponent<AttackControllerBase>();

        stateMachine.Initialize(_input, _movementBase, _anim);

        _agent.stoppingDistance = data.AttackRange;
    }

    private void Start()
    {
        GainMovementBehaviour();
        GainAttackBehaviour();
        GainDeathBehaviour();

        stateMachine.SetInitialState(typeof(MovementState));
        AttackController.SetSelectedMeleeAttack(stateMachine);
    }

    public void GainMovementBehaviour()
    {
        stateMachine.AddMovementState();
    }

    public void LoseMovementBehaviour()
    {
        stateMachine.RemoveState(typeof(MovementState));
    }

    public void GainAttackBehaviour()
    {
        stateMachine.AddAttackState(AttackController);
    }

    public void GainDeathBehaviour()
    {
        stateMachine.AddDeathState();
    }

    public void GainDamageTakenBehaviour()
    {
        stateMachine.AddDamageTakenState(data.DamageTakenDuration);
    }

    private void Attack()
    {
        stateMachine.ChangeState(typeof(AttackState));
    }

    protected void TakeDamage(int i)
    {
        stateMachine.ChangeState(typeof(DamageTakenState));
    }

    protected void Die()
    {
        _input.enabled = false;
        _agent.enabled = false;
        stateMachine.ChangeState(typeof(DeathState));
    }

    private void OnEnable()
    {
        _input.OnAttackActionStarted += Attack;

        _healthManager.OnDamageTaken += TakeDamage;
        _healthManager.OnDeath += Die;
    }

    private void OnDisable()
    {
        _input.OnAttackActionStarted -= Attack;

        _healthManager.OnDamageTaken -= TakeDamage;
        _healthManager.OnDeath -= Die;
    }
}
