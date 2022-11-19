using _Main.Scripts.GamePlay.AttackSystem;
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

        protected NavMeshAgent agent;
        protected AttackController attackController;
        protected AttackSelectorBase attackSelector;

        protected Vector3 startPos;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            attackController = GetComponent<AttackController>();
            attackSelector = GetComponent<AttackSelectorBase>();

            startPos = transform.position;
        }

        public override Vector3 GetLookInput()
        {
            if (attackSelector.Target != null)
            {
                return (attackSelector.Target.position - transform.position).normalized;
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


        protected virtual bool TargetIsInAttackRange()
        {
            return Vector3.Distance(transform.position, attackSelector.Target.position) <= attackController.SelectedMeleeAttack.CurrentComboDamageData.attackRange;
        }

        protected virtual bool TargetIsInLineOfSight()
        {
            bool lineOfSightCondition = false;

            var rayOrigin = transform.position + transform.up;
            var rayDirection = attackSelector.Target.position - transform.position;
            Ray ray = new Ray(rayOrigin, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, 15f))
            {
                if (hit.transform == attackSelector.Target)
                {
                    lineOfSightCondition = true;
                }
            }
            return lineOfSightCondition;
        }
    }
}