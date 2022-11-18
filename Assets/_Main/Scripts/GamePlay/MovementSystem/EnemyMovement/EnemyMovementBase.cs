using _Main.Scripts.GamePlay.BehaviourSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GridBrushBase;

namespace _Main.Scripts.GamePlay.MovementSystem
{
    public class EnemyMovementBase : MovementBase
    {
        private NavMeshAgent agent;
        private EnemyBehaviourData enemyBehaviourData;
        private IEnumerator moveOverTimeCo;

        public override Vector3 GetVelocity() => agent.velocity;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            enemyBehaviourData = GetComponent<EnemyBehaviourBase>().Data;
        }

        protected void MoveInDirection(Vector3 dir, float speed)
        {
            agent.speed = speed;
            agent.Move(agent.speed * Time.deltaTime * dir);
        }

        protected void RotateInDirection(Vector3 direction, float rotationSpeed)
        {
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
            }
        }

        public override void Move(Vector3 targetPos, float movementSpeedMultiplier, float rotationSpeedMultiplier)
        {
            if (!agent.isActiveAndEnabled) return;
            if (canMove && movementSpeedMultiplier != 0)
            {
                agent.speed = enemyBehaviourData.MovementSpeed * movementSpeedMultiplier;
                agent.SetDestination(targetPos);
            }
            else
            {
                agent.SetDestination(transform.position);
            }

            if (targetPos != Vector3.zero && movementSpeedMultiplier == 0)
                RotateInDirection(targetPos, enemyBehaviourData.RotationSpeed * rotationSpeedMultiplier);
        }

        public override void MoveOverTime(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true, float jumpHeight = 0f, AnimationCurve curve = null)
        {
            if (moveOverTimeCo != null)
            {
                StopCoroutine(moveOverTimeCo);
            }
            if (curve == null)
            {
                moveOverTimeCo = MoveOverTimeCo(endPos, duration, setDelay, useGravity);
                StartCoroutine(moveOverTimeCo);
            }
            else
            {
                moveOverTimeCo = MoveOverTimeWithAnimationCo(endPos, duration, setDelay, jumpHeight, curve);
                StartCoroutine(moveOverTimeCo);
            }

        }

        private IEnumerator MoveOverTimeCo(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true)
        {
            yield return new WaitForSeconds(setDelay);
            var startPos = transform.position;
            var dir = endPos - startPos;
            var timePassed = 0f;

            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;
                MoveInDirection(dir.normalized, dir.magnitude / duration);
                yield return null;
            }
            agent.speed = 4f;
        }

        private IEnumerator MoveOverTimeWithAnimationCo(Vector3 endPos, float duration, float setDelay = 0f, float jumpHeight = 0f, AnimationCurve curve = null)
        {
            yield return new WaitForSeconds(setDelay);
            agent.enabled = false;
            var startPos = transform.position;
            var timePassed = 0f;

            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos + new Vector3(0f, jumpHeight * curve.Evaluate(timePassed / duration), 0f), timePassed / duration);
                yield return null;
            }
            agent.enabled = true;
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, agent.areaMask))
            {
                agent.Warp(hit.position);
            }
        }

        protected override bool IsMoving()
        {
            return !agent.isStopped;
        }
    }
}