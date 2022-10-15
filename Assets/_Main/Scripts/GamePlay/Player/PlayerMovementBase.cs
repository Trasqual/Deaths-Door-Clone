using _Main.Scripts.GamePlay.MovementSystem;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerMovementBase : MovementBase
    {
        [SerializeField] private float baseMovementSpeed = 5f;
        [SerializeField] private float baseRotationSpeed = 20f;
        [SerializeField] private float fallToDeathTime = 2f;

        private Player player;
        private AnimationMovement animationMovement;

        private bool applyGravity = true;

        private float fallTimer = 0f;

        private IEnumerator moveOverTimeCo;

        private void Start()
        {
            player = GetComponent<Player>();
            animationMovement = GetComponentInChildren<AnimationMovement>();
        }

        private void Update()
        {
            if (!player.Controller.isGrounded && applyGravity)
            {
                fallTimer += Time.deltaTime;
                if (fallTimer >= fallToDeathTime)
                {
                    canMove = false;
                    applyGravity = false;
                    transform.position = new Vector3(-13f, 1f, 0f);
                    fallTimer = 0f;
                    DOVirtual.DelayedCall(1f, () => { canMove = true; applyGravity = true; });
                }
            }
            else
            {
                fallTimer = 0f;
            }
        }

        protected override bool IsMoving()
        {
            return player.Controller.velocity.magnitude > 0f;
        }



        protected void MoveInDirection(Vector3 dir, float speed)
        {
            player.Controller.Move(dir * Time.deltaTime * speed);
        }

        protected void RotateInDirection(Vector3 direction, float rotationSpeed)
        {
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
            }
        }

        private void ApplyGravity()
        {
            player.Controller.Move(Physics.gravity * Time.deltaTime);
        }

        public override void Move(Vector3 dir, float movementSpeedMultiplier, float rotationSpeedMultiplier)
        {
            if (canMove)
            {
                MoveInDirection(dir, baseMovementSpeed * movementSpeedMultiplier);
            }

            if (canRotate)
            {
                RotateInDirection(dir, baseRotationSpeed * rotationSpeedMultiplier);
            }

            if (applyGravity)
            {
                ApplyGravity();
            }
        }

        public override void MoveOverTime(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true, bool useAnimationMovement = false)
        {
            if (moveOverTimeCo != null)
            {
                StopCoroutine(moveOverTimeCo);
            }
            if (useAnimationMovement)
            {
                moveOverTimeCo = MoveOverTimeWithAnimationCo(duration, useGravity);
                StartCoroutine(moveOverTimeCo);
            }
            else
            {
                moveOverTimeCo = MoveOverTimeCo(endPos, duration, setDelay, useGravity);
                StartCoroutine(moveOverTimeCo);
            }
        }

        private IEnumerator MoveOverTimeCo(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true)
        {
            applyGravity = useGravity;
            yield return new WaitForSeconds(setDelay);
            var startPos = transform.position;
            var dir = endPos - startPos;
            var timePassed = 0f;

            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;
                MoveInDirection(dir.normalized, (dir.magnitude / duration));
                if (useGravity)
                {
                    ApplyGravity();
                }
                yield return null;
            }
            applyGravity = true;
        }

        private IEnumerator MoveOverTimeWithAnimationCo(float duration, bool useGravity)
        {
            applyGravity = useGravity;
            animationMovement.Activate();
            yield return new WaitForSeconds(duration);
            animationMovement.DeActivate();
            applyGravity = true;
        }
    }
}