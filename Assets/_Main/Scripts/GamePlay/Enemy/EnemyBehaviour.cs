using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : BehaviourBase
{
    NavMeshAgent _agent;
    HealthComponentBase _healthManager;
    MovementBase _movementBase;
    Animator _anim;
    InputBase _input;

    protected override void Awake()
    {
        base.Awake();
        _healthManager = GetComponent<HealthComponentBase>();
        _agent = GetComponent<NavMeshAgent>();
        _movementBase = GetComponent<MovementBase>();
        _anim = GetComponentInChildren<Animator>();
        _input = GetComponent<InputBase>();

        stateMachine.Initialize(_input, _movementBase, _anim, _healthManager);
    }

    private void Start()
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

    protected void Die()
    {
        _agent.isStopped = true;
        stateMachine.ChangeState(typeof(DeathState));
    }

    private void OnEnable()
    {
        _healthManager.OnDeath += Die;
    }
}