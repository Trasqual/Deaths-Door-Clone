using _Main.Scripts.GamePlay.MovementSystem;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementBase : MovementBase
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Move(Vector3 dir, float movementSpeedMultiplier, float rotationSpeedMultiplier)
    {
        agent.SetDestination(dir);
    }

    public override void MoveOverTime(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true, bool useAnimationMovement = false)
    {

    }

    protected override bool IsMoving()
    {
        return !agent.isStopped;
    }
}
