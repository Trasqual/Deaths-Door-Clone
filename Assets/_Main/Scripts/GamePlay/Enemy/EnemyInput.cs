using _Main.Scripts.GamePlay.InputSystem;
using UnityEngine;

public class EnemyInput : InputBase
{
    public override Vector3 GetLookInput()
    {
        return Vector3.zero;
    }

    public override Vector3 GetMovementInput()
    {
        return Vector3.zero;
    }
}
