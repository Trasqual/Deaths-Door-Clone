using _Main.Scripts.GamePlay.InputSystem;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyInputBase : InputBase
{
    [SerializeField] protected bool shouldPatrol = false;
    [SerializeField] protected float patrolRange = 6f;
    [SerializeField] protected float patrolDuration = 3f;
    protected float patrolTimer = 0f;
    protected Vector3 patrolPosition;

    [SerializeField] protected EnemyTriggerDetector detecterPrefab;
    protected EnemyTriggerDetector detector;
    protected NavMeshAgent agent;

    protected IDamageable _target;
    protected Vector3 startPos;
    protected float originalStoppingDistance;

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

    protected virtual void Start()
    {
        originalStoppingDistance = agent.stoppingDistance;
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
            agent.stoppingDistance = originalStoppingDistance;
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
            agent.stoppingDistance = 0.5f;
            var returnPos = shouldPatrol ? GetPatrolPosition() : startPos;
            if (Vector3.Distance(transform.position, returnPos) > agent.stoppingDistance)
            {
                return returnPos;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    protected virtual Vector3 GetPatrolPosition()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolDuration)
        {
            var randPos = Random.insideUnitCircle * patrolRange;
            patrolPosition = startPos + new Vector3(randPos.x, 0f, randPos.y);
            patrolTimer = 0f;
        }
        return patrolPosition;
    }

    protected abstract void Update();

    protected virtual void OnTargetDetectedCallback(IDamageable target)
    {
        _target = target;
    }

    protected virtual void OnTargetLostCallback()
    {
        _target = null;
    }
}
