using _Main.Scripts.GamePlay.MovementSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementBase : MovementBase
{
    private NavMeshAgent _agent;
    private AnimationMovementBase _animationMovement;
    private IEnumerator _moveOverTimeCo;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animationMovement = GetComponentInChildren<AnimationMovementBase>();
    }

    protected void MoveInDirection(Vector3 dir, float speed)
    {
        _agent.speed = speed;
        _agent.Move(transform.position + dir);
    }

    public override void Move(Vector3 dir, float movementSpeedMultiplier, float rotationSpeedMultiplier)
    {
        if(_agent.isActiveAndEnabled)
        _agent.SetDestination(dir);
    }

    public override void MoveOverTime(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true, bool useAnimationMovement = false)
    {
        if (_moveOverTimeCo != null)
        {
            StopCoroutine(_moveOverTimeCo);
        }
        if (useAnimationMovement)
        {
            _moveOverTimeCo = MoveOverTimeWithAnimationCo(duration, useGravity);
            StartCoroutine(_moveOverTimeCo);
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
        _agent.speed = 4f;
    }

    private IEnumerator MoveOverTimeWithAnimationCo(float duration, bool useGravity)
    {
        _animationMovement.Activate();
        _agent.enabled = false;
        yield return new WaitForSeconds(duration);
        _animationMovement.DeActivate();
        _agent.enabled = true;
    }

    protected override bool IsMoving()
    {
        return !_agent.isStopped;
    }
}
