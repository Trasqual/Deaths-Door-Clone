using _Main.Scripts.GamePlay.MovementSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementBase : MovementBase
{
    private NavMeshAgent agent;
    private AnimationMovementBase animationMovement;
    private IEnumerator moveOverTimeCo;

    public override Vector3 GetVelocity() => agent.velocity;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animationMovement = GetComponentInChildren<AnimationMovementBase>();
    }

    protected void MoveInDirection(Vector3 dir, float speed)
    {
        agent.speed = speed;
        agent.Move(transform.position + dir);
    }

    public override void Move(Vector3 dir, float movementSpeedMultiplier, float rotationSpeedMultiplier)
    {
        if (!agent.isActiveAndEnabled) return;
        agent.SetDestination(dir);
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeedMultiplier);
    }

    public override void MoveOverTime(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true, bool useAnimationMovement = false)
    {
        if (moveOverTimeCo != null)
        {
            StopCoroutine(moveOverTimeCo);
        }
        if (useAnimationMovement)
        {
            moveOverTimeCo = MoveOverTimeWithAnimationCo(duration, useGravity);
            StartCoroutine(moveOverTimeCo);
        }
        //else
        //{
        //    moveOverTimeCo = MoveOverTimeCo(endPos, duration, setDelay, useGravity);
        //    StartCoroutine(moveOverTimeCo);
        //}
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
            MoveInDirection(dir.normalized, (dir.magnitude / duration));
            yield return null;
        }
        agent.speed = 4f;
    }

    private IEnumerator MoveOverTimeWithAnimationCo(float duration, bool useGravity)
    {
        animationMovement.Activate();
        agent.enabled = false;
        yield return new WaitForSeconds(duration);
        animationMovement.DeActivate();
        agent.enabled = true;
    }

    protected override bool IsMoving()
    {
        return !agent.isStopped;
    }
}
