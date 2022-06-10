using _Main.Scripts.GamePlay.Movement;
using DG.Tweening;
using UnityEngine;

public class RollHandler : MonoBehaviour
{
    [SerializeField] float rollSpeed = 10f;
    [SerializeField] float rollDuration = 1.5f;

    private InputBase input;
    private Movement movement;
    private AnimationBase anim;

    private Vector3 rollDirection;

    bool isRolling;

    private void Awake()
    {
        input = GetComponent<InputBase>();
        movement = GetComponent<Movement>();
        anim = GetComponent<AnimationBase>();

        input.OnRollAction += StartRoll;
    }

    private void StartRoll()
    {
        if (isRolling) return;
        if(movement.IsInSpecialAction) return;
        movement.IsInSpecialAction = true;
        rollDirection = input.GetMovementInput();
        if (rollDirection != Vector3.zero) transform.rotation = Quaternion.LookRotation(rollDirection);
        isRolling = true;
        movement.StopMovementAndRotation();
        anim.PlayRollAnim();
        DOVirtual.DelayedCall(rollDuration, () => StopRoll());
    }

    private void StopRoll()
    {
        isRolling = false;
        movement.IsInSpecialAction = false;
        movement.StartMovementAndRotation();
    }

    private void Update()
    {
        if (isRolling)
        {
            movement.MoveInDirection(rollDirection == Vector3.zero ? transform.forward : rollDirection.normalized, rollSpeed);
        }
    }
}
