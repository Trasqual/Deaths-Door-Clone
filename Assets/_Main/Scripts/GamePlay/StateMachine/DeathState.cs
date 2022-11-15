using _Main.Scripts.GamePlay.MovementSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachineSystem
{
    public class DeathState : StateBase, ITransition, IAnimation
    {
        private MovementBase _movementBase;

        public void Initialize(MovementBase movementBase, Animator animator)
        {
            _movementBase = movementBase;
            Animator = animator;

            _transition = this;
        }
        public override void EnterState()
        {
            _movementBase.StopMovementAndRotation();
            Animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
        }

        public override void ExitState()
        {
            throw new NotImplementedException();
        }

        public override void UpdateState()
        {

        }

        public override void CancelState()
        {
            throw new NotImplementedException();
        }

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

        public void PlayAnimation()
        {
            Animator.SetBool(HashCode, true);
        }

        public void StopAnimation()
        {
            Animator.SetBool(HashCode, false);
        }

        #endregion

    }
}