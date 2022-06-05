using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        private Player player;

        private bool canMove = true;

        private void Start()
        {
            player = GetComponent<Player>();
            anim = GetComponentInChildren<Animator>();

            player.Input.OnRollButtonPressed += PlayRollAnim;
            player.Input.OnAimButtonPressed += StopMovement;
            player.Input.OnAimButtonReleased += StartMovement;
        }

        private void PlayMovementAnim()
        {
            anim.SetFloat("movement", transform.InverseTransformDirection(player.Input.GetMovementInput()).magnitude, 0.1f, Time.deltaTime);
        }

        private void PlayAimAnim()
        {
            anim.SetBool("isAiming", player.Input.IsAiming);
        }

        private void PlayRollAnim()
        {
            anim.SetTrigger("roll");
        }

        private void StopMovement()
        {
            canMove = false;
        }

        private void StartMovement()
        {
            canMove = true;
        }

        private void Update()
        {
            PlayAimAnim();
            if (canMove)
            {
                PlayMovementAnim();
            }
        }
    }
}