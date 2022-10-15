using UnityEngine;

public class AnimationMovement : MonoBehaviour
{
    Animator anim;
    CharacterController characterController;

    bool isActive;

    private void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponentInParent<CharacterController>();
    }

    private void OnAnimatorMove()
    {
        if (!isActive) return;
        characterController.Move(anim.deltaPosition);
    }

    public void Activate()
    {
        isActive = true;
    }

    public void DeActivate()
    {
        isActive = false;
    }
}
