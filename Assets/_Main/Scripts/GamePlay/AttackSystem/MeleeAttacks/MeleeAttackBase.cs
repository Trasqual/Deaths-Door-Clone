using _Main.Scripts.GamePlay.AttackSystem;
using DG.Tweening;
using UnityEngine;

public class MeleeAttackBase : AttackBase
{
    bool canAttack = true;
    Tween attackDelay;

    int comboCount => attackAnimationDatas.Length;
    int currentComboCount;
    bool hasCombo => attackAnimationDatas.Length > 1;
    bool canCombo;
    Tween comboDelay;

    private void Awake()
    {
        AssignAnimationData();
    }

    //Called when attack button is down
    protected override void DoOnActionStart()
    {
        if (!canAttack)
        {
            EndAttack();
            return;
        }

        if (canCombo)
        {
            comboDelay?.Kill();
        }
        if (hasCombo)
        {
            if (currentComboCount < comboCount - 1)
            {
                var data = (MeleeAttackAnimationData)CurrentAttackAnimationData;
                canCombo = true;
                currentComboCount++;
                AssignAnimationData();
                comboDelay = DOVirtual.DelayedCall(data.attackCD, () =>
                {
                    canCombo = false;
                    currentComboCount = 0;
                    AssignAnimationData();
                    EndAttack();
                });
            }
            else
            {
                EndAttack();
            }
        }
        else
        {
            EndAttack();
        }
    }

    //Called when attack button is up
    protected override void DoOnActionEnd()
    {

    }

    //Called when attack state is canceled
    protected override void DoOnActionCanceled()
    {
        comboDelay?.Kill();
        attackDelay?.Kill();
        canAttack = true;
        canCombo = false;
        currentComboCount = 0;
    }

    //Called when final attack is used
    private void EndAttack()
    {
        if (canAttack)
        {
            var data = (MeleeAttackAnimationData)CurrentAttackAnimationData;
            attackDelay = DOVirtual.DelayedCall(data.attackCD, () => canAttack = true);
        }
        canAttack = false;
        OnAttackCompleted?.Invoke();
    }

    private void AssignAnimationData()
    {
        CurrentAttackAnimationData = attackAnimationDatas[currentComboCount];
    }
}
