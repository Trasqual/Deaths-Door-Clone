using _Main.Scripts.GamePlay.MovementSystem;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerMovementBase : MovementBase
    {
        [SerializeField] private float _baseMovementSpeed = 5f;
        [SerializeField] private float _baseRotationSpeed = 20f;
        [SerializeField] private float _fallToDeathTime = 2f;

        private Player _player;
        private AnimationMovementBase _animationMovement;

        private bool _applyGravity = true;

        private float _fallTimer = 0f;

        private IEnumerator _moveOverTimeCo;

        private void Start()
        {
            _player = GetComponent<Player>();
            _animationMovement = GetComponentInChildren<AnimationMovementBase>();
        }

        private void Update()
        {
            if (!_player.Controller.isGrounded && _applyGravity)
            {
                _fallTimer += Time.deltaTime;
                if (_fallTimer >= _fallToDeathTime)
                {
                    _canMove = false;
                    _applyGravity = false;
                    transform.position = new Vector3(-10f, 1f, 3.35f);
                    _fallTimer = 0f;
                    DOVirtual.DelayedCall(1f, () => { _canMove = true; _applyGravity = true; });
                }
            }
            else
            {
                _fallTimer = 0f;
            }
        }

        protected override bool IsMoving()
        {
            return _player.Controller.velocity.magnitude > 0f;
        }



        protected void MoveInDirection(Vector3 dir, float speed)
        {
            _player.Controller.Move(dir * Time.deltaTime * speed);
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
            _player.Controller.Move(Physics.gravity * Time.deltaTime);
        }

        public override void Move(Vector3 dir, float movementSpeedMultiplier, float rotationSpeedMultiplier)
        {
            if (_canMove)
            {
                MoveInDirection(dir, _baseMovementSpeed * movementSpeedMultiplier);
            }

            if (_canRotate)
            {
                RotateInDirection(dir, _baseRotationSpeed * rotationSpeedMultiplier);
            }

            if (_applyGravity)
            {
                ApplyGravity();
            }
        }

        public override void MoveOverTime(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true, bool useAnimationMovement = false)
        {
            if (_moveOverTimeCo != null)
            {
                StopCoroutine(_moveOverTimeCo);
            }
            if (useAnimationMovement)
            {
                _moveOverTimeCo = MoveOverTimeWithAnimationCo(duration, useGravity);
                StartCoroutine(_moveOverTimeCo);
            }
            else
            {
                _moveOverTimeCo = MoveOverTimeCo(endPos, duration, setDelay, useGravity);
                StartCoroutine(_moveOverTimeCo);
            }
        }

        private IEnumerator MoveOverTimeCo(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true)
        {
            _applyGravity = useGravity;
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
            _applyGravity = true;
        }

        private IEnumerator MoveOverTimeWithAnimationCo(float duration, bool useGravity)
        {
            _applyGravity = useGravity;
            _animationMovement.Activate();
            yield return new WaitForSeconds(duration);
            _animationMovement.DeActivate();
            _applyGravity = true;
        }
    }
}