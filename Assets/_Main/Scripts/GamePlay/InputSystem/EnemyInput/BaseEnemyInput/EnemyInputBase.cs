using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.DetectionSystem;
using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;
using UnityEngine.AI;

namespace _Main.Scripts.GamePlay.InputSystem
{
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
        protected AttackController attackController;

        protected IDamageable _target;
        protected Vector3 startPos;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            attackController = GetComponent<AttackController>();
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
            return Vector3.zero;
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

        protected virtual bool TargetIsInAttackRange()
        {
            return false;
        }

        protected virtual bool TargetIsInLineOfSight()
        {
            bool lineOfSightCondition = false;

            var targetTransform = _target.GetTransform();

            var rayOrigin = transform.position + transform.up;
            var rayDirection = targetTransform.position - transform.position;
            Ray ray = new Ray(rayOrigin, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, 15f))
            {
                if (hit.transform == targetTransform)
                {
                    lineOfSightCondition = true;
                }
            }
            return lineOfSightCondition;
        }
    }
}