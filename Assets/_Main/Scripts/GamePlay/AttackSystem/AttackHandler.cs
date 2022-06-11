using _Main.Scripts.GamePlay.Movement;
using System.Collections;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] float attackSpeed = 0.5f;

    InputBase input;
    AnimationBase anim;
    Movement movement;

    bool isAttacking;
    Vector3 attackDirection;

    private void Start()
    {
        input = GetComponent<InputBase>();
        anim = GetComponent<AnimationBase>();
        movement = GetComponent<Movement>();

        input.OnAttackAction += Attack;
    }

    private void Attack()
    {
        if (isAttacking) return;
        if (movement.IsInSpecialAction) return;
        movement.StopMovementAndRotation();
        attackDirection = input.GetLookInput();
        transform.rotation = Quaternion.LookRotation(attackDirection);

        isAttacking = true;
        anim.PlayAttackAnim();
        StartCoroutine(AttackCD());
    }

    IEnumerator AttackCD()
    {
        var t = 0f;
        while (t < attackSpeed)
        {
            t += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
        movement.StartMovementAndRotation();
    }
}
