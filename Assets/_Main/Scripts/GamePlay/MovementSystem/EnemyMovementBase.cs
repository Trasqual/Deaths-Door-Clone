using _Main.Scripts.GamePlay.MovementSystem;
using UnityEngine;

public class EnemyMovementBase : MovementBase
{
    public override void Move(Vector3 dir, float movementSpeedMultiplier, float rotationSpeedMultiplier)
    {

    }

    protected override bool IsMoving()
    {
        return false;
    }
}
