using UnityEngine;

public class AnimationBase : MonoBehaviour
{
    [SerializeField] protected Animator anim;

    private static readonly int movementHash = Animator.StringToHash("movement");
    private static readonly int attackHash = Animator.StringToHash("attack");
    private static readonly int isAimingHash = Animator.StringToHash("isAiming");
    private static readonly int isCanceledHash = Animator.StringToHash("isCanceled");
    private static readonly int rollHash = Animator.StringToHash("roll");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public virtual void PlayMovementAnim(Vector3 dir)
    {
        anim.SetFloat(movementHash, transform.InverseTransformDirection(dir).magnitude, 0.1f, Time.deltaTime);
    }

    public virtual void PlayAttackAnim()
    {
        anim.SetTrigger(attackHash);
    }

    public void PlayAimAnim(bool isAiming, bool isCanceled)
    {
        anim.SetBool(isAimingHash, isAiming);
        anim.SetBool(isCanceledHash, isCanceled);
    }

    public void PlayRollAnim()
    {
        anim.SetTrigger(rollHash);
    }
}
