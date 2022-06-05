using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] Animator anim;
        Player player;

        private void Start()
        {
            player = GetComponent<Player>();
            anim = GetComponentInChildren<Animator>();

            player.Input.OnRollButtonPressed += PlayRollAnim;
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

        private void Update()
        {
            PlayAimAnim();
            if (!player.Input.IsAiming)
            {
                PlayMovementAnim();
            }
        }
    }
}