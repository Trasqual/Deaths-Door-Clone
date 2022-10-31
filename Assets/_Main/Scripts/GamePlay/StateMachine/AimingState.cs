using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.ActionSystem;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public class AimingState : StateBase, IAction, ITransition, IAnimationOverridable
    {
        public bool IsAiming { get; private set; }

        private InputBase _input;
        private MovementBase _movementBase;
        private float _aimSpeedMultiplier;
        private float _recoilDelay;
        private Tween _recoilDelayTween;
        private AttackControllerBase _attackController;
        public Action OnComplete;

        public void Initialize(InputBase input, MovementBase movementBase, Animator animator, float aimSpeedMultiplier, float recoilDelay, AttackControllerBase attackController)
        {
            _input = input;
            _movementBase = movementBase;
            Animator = animator;
            _aimSpeedMultiplier = aimSpeedMultiplier;
            _recoilDelay = recoilDelay;
            _attackController = attackController;
            _transition = this;
            OriginalController = Animator.runtimeAnimatorController;

            _input.OnAimActionEnded += EndAim;
            _transition.AddTransition(typeof(MovementState), () => !IsAiming, () => false);
            _transition.AddTransition(typeof(DodgeState), () => true, () => true);
            _transition.AddTransition(typeof(DeathState), () => true, () => true);
            _transition.AddTransition(typeof(DamageTakenState), () => true, () => true);
        }

        public override void EnterState()
        {
            OnActionStart?.Invoke();
            PlayAnimation();
            _movementBase.StartMovementAndRotation();
            IsAiming = true;
        }

        public override void UpdateState()
        {
            _movementBase.Move(_input.GetLookInput(), 0f, _aimSpeedMultiplier);
        }

        public void EndAim()
        {
            if (!IsAiming) return;
            StopAnimation();
            OnActionEnd?.Invoke();
            _recoilDelayTween = DOVirtual.DelayedCall(_recoilDelay, () =>
            {
                IsAiming = false;
                OnComplete?.Invoke();
            });
        }

        public override void ExitState()
        {
            _movementBase.StopMovementAndRotation();
        }

        public override void CancelState()
        {
            OnActionCanceled?.Invoke();
            IsAiming = false;
            _recoilDelayTween.Kill();
            StopAnimation();
            _movementBase.StopMovementAndRotation();
        }

        #region Actions

        public event Action OnActionStart;
        public event Action OnActionEnd;
        public event Action OnActionCanceled;

        #endregion

        #region Transition

        private ITransition _transition = null;
        public List<Transition> Transitions { get; private set; } = new();
        public bool TryGetTransition(Type to, out Transition targetTransition)
        {
            foreach (var transition in Transitions)
            {
                if (transition.To == to)
                {
                    targetTransition = transition;
                    return true;
                }
            }

            targetTransition = null;
            return false;
        }

        #endregion

        #region Animation

        public int HashCode { get; private set; } = Animator.StringToHash("isAiming");
        public Animator Animator { get; private set; } = null;
        public RuntimeAnimatorController OriginalController { get; private set; } = null;

        public void SetAnimatorOverrideController()
        {
            Animator.runtimeAnimatorController = _attackController.SelectedRangedAttack.CurrentComboAnimationData.overrideController;
        }

        public void ResetAnimatorController()
        {
            Animator.runtimeAnimatorController = OriginalController;
        }

        public void PlayAnimation()
        {
            SetAnimatorOverrideController();
            Animator.SetBool(HashCode, true);
        }

        public void StopAnimation()
        {
            ResetAnimatorController();
            Animator.SetBool(HashCode, false);
        }

        #endregion
    }
}
