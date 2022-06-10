using UnityEngine;

public class AnimationBase : MonoBehaviour
{
    [SerializeField] protected Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public virtual void PlayMovementAnim(Vector3 dir)
    {
        anim.SetFloat("movement", transform.InverseTransformDirection(dir).magnitude, 0.1f, Time.deltaTime);
    }

    public virtual void PlayAttackAnim()
    {
        anim.SetTrigger("attack");
    }

    public void PlayAimAnim(bool isAiming)
    {
        anim.SetBool("isAiming", isAiming);
    }

    public void PlayRollAnim()
    {
        anim.SetTrigger("roll");
    }
}
