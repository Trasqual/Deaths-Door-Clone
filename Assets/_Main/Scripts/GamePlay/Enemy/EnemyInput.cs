using _Main.Scripts.GamePlay.InputSystem;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInput : InputBase
{
    [SerializeField] private EnemyTriggerDetector detecterPrefab;
    private EnemyTriggerDetector detector;
    private NavMeshAgent agent;

    private IDamageable _target;
    private Vector3 startPos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (detector == null)
        {
            detector = Instantiate(detecterPrefab, transform.position, Quaternion.identity, transform);
            detector.OnTargetFound += OnTargetDetectedCallback;
            detector.OnTargetLost += OnTargetLostCallback;
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
            if (Vector3.Distance(transform.position, _target.GetTransform().position) > agent.stoppingDistance)
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
            if (Vector3.Distance(transform.position, startPos) > agent.stoppingDistance)
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
            if (Vector3.Distance(transform.position, _target.GetTransform().position) <= agent.stoppingDistance)
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
