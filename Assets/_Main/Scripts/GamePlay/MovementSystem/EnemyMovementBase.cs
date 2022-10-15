using _Main.Scripts.GamePlay.MovementSystem;
using UnityEngine;

public class EnemyMovementBase : MovementBase
{
    public override void Move(Vector3 dir, float movementSpeedMultiplier, float rotationSpeedMultiplier)
    {

    }

    public override void MoveOverTime(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true)
    {
        
    }

    protected override bool IsMoving()
    {
        return false;
    }
}
