using _Main.Scripts.GamePlay.MovementSystem;
using _Main.Scripts.GamePlay.StateMachine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageTakenState : StateBase, ITransition, IAnimation
{
    public Action OnComplete;

    private MovementBase _movementBase;
    private float _duration;
    private bool _durationComplete;

    public void Initialize(MovementBase movementBase, Animator animator, float damageTakenDuration)
    {
        _movementBase = movementBase;
        Animator = animator;
        _duration = damageTakenDuration;
        _transition = this;
        _transition.AddTransition(typeof(MovementState), () => _durationComplete, () => false);
    }

    public override void EnterState()
    {
        _movementBase.StopMovementAndRotation();
        _durationComplete = false;
        PlayAnimation();
        DOVirtual.DelayedCall(_duration, () =>
        {
            _durationComplete = true;
            OnComplete?.Invoke();
        });
    }

    public override void ExitState()
    {
        _movementBase.StopMovementAndRotation();
    }

    public override void UpdateState()
    {

    }

    public override void CancelState()
    {

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

    public int HashCode { get; private set; } = Animator.StringToHash("takeDamage");
    public Animator Animator { get; private set; } = null;

    public void PlayAnimation()
    {
        Animator.SetTrigger(HashCode);
    }

    public void StopAnimation()
    {

    }

    #endregion
}
