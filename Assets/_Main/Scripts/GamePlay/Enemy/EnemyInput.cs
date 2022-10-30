using _Main.Scripts.GamePlay.InputSystem;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInput : InputBase
{
    [SerializeField] private EnemyTriggerDetector _detecterPrefab;
    private EnemyTriggerDetector _detector;
    private NavMeshAgent _agent;

    private IDamageable _target;
    private Vector3 startPos;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (_detector == null)
        {
            _detector = Instantiate(_detecterPrefab, transform.position, Quaternion.identity, transform);
            _detector.OnTargetFound += OnTargetDetectedCallback;
            _detector.OnTargetLost += OnTargetLostCallback;
        }

        startPos = transform.position;
    }

    public override Vector3 GetLookInput()
    {
        if (_target != null)
        {
            return (_target.GetTransform().position - transform.position).normalized;
        }
        else
        {
            return (startPos - transform.position).normalized;
        }
    }

    public override Vector3 GetMovementInput()
    {
        if (_target != null)
        {
            if (Vector3.Distance(transform.position, _target.GetTransform().position) > _agent.stoppingDistance)
            {
                return _target.GetTransform().position;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, startPos) > _agent.stoppingDistance)
            {
                return startPos;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    private void Update()
    {
        if (_target != null)
        {
            if (Vector3.Distance(transform.position, _target.GetTransform().position) <= _agent.stoppingDistance)
            {
                OnAttackActionStarted?.Invoke();
            }
        }
    }

    private void OnTargetDetectedCallback(IDamageable target)
    {
        _target = target;
    }

    private void OnTargetLostCallback()
    {
        _target = null;
    }
}
