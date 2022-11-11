using _Main.Scripts.GamePlay.MovementSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourBase : BehaviourBase<EnemyBehaviourData>
{
    protected NavMeshAgent _agent;
    protected HealthComponentBase _healthManager;
    protected MovementBase _movementBase;
    protected Animator _anim;

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
    }

    protected virtual void Start()
    {
        GainMovementBehaviour();
        GainDeathBehaviour();

        stateMachine.SetInitialState(typeof(MovementState));
    }

    public void GainMovementBehaviour()
    {
        stateMachine.AddMovementState();
    }

    public void LoseMovementBehaviour()
    {
        stateMachine.RemoveState(typeof(MovementState));
    }

    public void GainDeathBehaviour()
    {
        stateMachine.AddDeathState();
    }

    protected void TakeDamage(int i)
    {
        //stateMachine.ChangeState(typeof(DamageTakenState));
    }

    protected void Die()
    {
        _input.enabled = false;
        _agent.enabled = false;
        stateMachine.ChangeState(typeof(DeathState));
    }

    protected virtual void OnEnable()
    {
        _healthManager.OnDamageTaken += TakeDamage;
        _healthManager.OnDeath += Die;
    }

    protected virtual void OnDisable()
    {
        _healthManager.OnDamageTaken -= TakeDamage;
        _healthManager.OnDeath -= Die;
    }
}
