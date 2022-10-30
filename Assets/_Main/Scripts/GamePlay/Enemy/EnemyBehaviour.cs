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

        _stateMachine.Initialize(_input, _movementBase, _anim);

        _agent.stoppingDistance = _data.AttackRange;
    }

    private void Start()
    {
        GainMovementBehaviour();
        GainAttackBehaviour();
        GainDamageTakenBehaviour();
        GainDeathBehaviour();

        _stateMachine.SetInitialState(typeof(MovementState));
        AttackController.SetSelectedMeleeAttack(_stateMachine);
    }

    public void GainMovementBehaviour()
    {
        _stateMachine.AddMovementState();
    }

    public void LoseMovementBehaviour()
    {
        _stateMachine.RemoveState(typeof(MovementState));
    }

    public void GainAttackBehaviour()
    {
        _stateMachine.AddAttackState(AttackController);
    }

    public void GainDeathBehaviour()
    {
        _stateMachine.AddDeathState();
    }

    public void GainDamageTakenBehaviour()
    {
        _stateMachine.AddDamageTakenState(_data.DamageTakenDuration);
    }

    private void Attack()
    {
        _stateMachine.ChangeState(typeof(MeleeAttackState));
    }

    protected void TakeDamage(int i)
    {
        _stateMachine.ChangeState(typeof(DamageTakenState));
    }

    protected void Die()
    {
        _input.enabled = false;
        _agent.enabled = false;
        _stateMachine.ChangeState(typeof(DeathState));
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
