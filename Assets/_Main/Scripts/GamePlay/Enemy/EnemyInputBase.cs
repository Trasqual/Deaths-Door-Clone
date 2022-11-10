using _Main.Scripts.GamePlay.InputSystem;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyInputBase : InputBase
{
    [SerializeField] protected EnemyTriggerDetector detecterPrefab;
    protected EnemyTriggerDetector detector;
    protected NavMeshAgent agent;

    protected IDamageable _target;
    protected Vector3 startPos;

    protected virtual void Awake()
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

    protected abstract void Update();

    protected void OnTargetDetectedCallback(IDamageable target)
    {
        _target = target;
    }

    protected void OnTargetLostCallback()
    {
        _target = null;
    }
}
