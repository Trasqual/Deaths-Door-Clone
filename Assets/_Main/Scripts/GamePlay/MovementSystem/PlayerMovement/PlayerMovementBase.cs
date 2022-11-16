using _Main.Scripts.GamePlay.BehaviourSystem;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.MovementSystem
{
    public class PlayerMovementBase : MovementBase
    {
        [SerializeField] private float baseMovementSpeed = 5f;
        [SerializeField] private float baseRotationSpeed = 20f;
        [SerializeField] private float fallToDeathTime = 2f;

        private PlayerBehaviour player;

        private bool applyGravity = true;

        private float fallTimer = 0f;

        private IEnumerator moveOverTimeCo;

        private Vector3 velocity;

        public override Vector3 GetVelocity()
        {
            return velocity;
        }

        private void Start()
        {
            player = GetComponent<PlayerBehaviour>();
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
                    transform.position = new Vector3(-10f, 1f, 3.35f);
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
            velocity = player.Controller.velocity;
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

        public override void MoveOverTime(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true, float jumpHeight = 0f, AnimationCurve curve = null)
        {
            if (moveOverTimeCo != null)
            {
                StopCoroutine(moveOverTimeCo);
            }
            moveOverTimeCo = MoveOverTimeCo(endPos, duration, setDelay, useGravity, jumpHeight, curve);
            StartCoroutine(moveOverTimeCo);
        }

        private IEnumerator MoveOverTimeCo(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true, float jumpHeight = 0f, AnimationCurve curve = null)
        {
            applyGravity = useGravity;
            yield return new WaitForSeconds(setDelay);
            var startPos = transform.position;
            var dir = Vector3.zero;
            var timePassed = 0f;

            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;
                if (useGravity)
                {
                    dir = endPos - startPos;
                }
                else
                {
                    dir = new Vector3(endPos.x, endPos.y + jumpHeight * curve.Evaluate(timePassed / duration), endPos.z) - new Vector3(startPos.x, transform.position.y, startPos.z);
                }
                MoveInDirection(dir.normalized, (dir.magnitude / duration));

                if (useGravity)
                {
                    ApplyGravity();
                }
                yield return null;
            }
            applyGravity = true;
        }
    }
}