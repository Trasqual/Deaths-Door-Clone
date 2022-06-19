using _Main.Scripts.GamePlay.Movement;
using DG.Tweening;
using System;
using UnityEngine;

public class AttackState : StateBase, IAction
{
    public event Action OnActionStart;
    public event Action OnActionEnd;
    public event Action OnActionCanceled;

    private readonly AnimationBase anim;
    private readonly InputBase input;
    private readonly Movement movement;
    private readonly float comboTimer = 0.3f;

    public AttackState(int priority, StateMachine stateMachine, AnimationBase anim, InputBase input, Movement movement, float comboTimer) : base(priority, stateMachine)
    {
        this.anim = anim;
        this.input = input;
        this.movement = movement;
        this.comboTimer = comboTimer;
    }

    private float comboCountdown = 0f;
    private bool canCombo;
    private int comboCount = 2;
    private Vector3 direction;
    Tween exitDelayTween;

    public bool IsAttackComplete { get; private set; }

    public override void EnterState()
    {
        IsAttackComplete = false;
        input.OnAttackAction += Combo;
        movement.StartMovementAndRotation();
        PerformAttack();
    }

    private void ExitIfNoCombo()
    {
        IsAttackComplete = true;
        stateMachine.ChangeState(stateMachine.MovementState);
    }

    private void Combo()
    {
        if (canCombo)
        {
            Debug.Log(comboCount);
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (exitDelayTween != null) exitDelayTween.Kill();
        OnActionStart?.Invoke();
        anim.PlayAttackAnim(comboCount);
        direction = input.GetMovementInput() == Vector3.zero ? stateMachine.transform.forward : input.GetMovementInput().normalized;
        stateMachine.transform.forward = direction;
        exitDelayTween = DOVirtual.DelayedCall(comboTimer + 0.1f, () => ExitIfNoCombo());
        comboCount--;
        comboCountdown = 0f;
        canCombo = true;
        if (comboCount < 0)
        {
            input.OnAttackAction -= Combo;
        }
    }

    public override void UpdateState()
    {
        comboCountdown += Time.deltaTime;
        if (comboCountdown >= comboTimer)
        {
            canCombo = false;
        }
    }

    public override void ExitState()
    {
        comboCount = 2;
        comboCountdown = 0f;
        canCombo = false;
        input.OnAttackAction -= Combo;
    }

    public override void CancelState()
    {
        exitDelayTween.Kill();
    }
}
