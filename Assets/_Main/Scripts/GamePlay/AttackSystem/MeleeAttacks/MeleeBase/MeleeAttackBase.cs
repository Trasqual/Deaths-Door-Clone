using _Main.Scripts.GamePlay.AttackSystem;
using DG.Tweening;
using UnityEngine;

public class MeleeAttackBase : AttackBase
{
    bool canAttack = true;
    Tween attackDelay;

    int comboCount => attackDatas.Count;
    int currentComboCount;
    bool hasCombo => attackDatas.Count > 1;
    bool canCombo;
    Tween comboDelay;

    protected virtual void Awake()
    {
        CurrentAttackAnimationData = attackDatas[0].AttackAnimationData;
    }

    //Called when attack button is down
    protected override void DoOnActionStart() //can move everything to DoOnActionEnd for charge attacks
    {
        //if (!canAttack && !canCombo) //if the attack is on CD and can't combo exit attack state
        //{
        //    EndAttack();
        //    return;
        //}
        if (!canAttack) //if the attack is on CD but can combo don't do anything
        {
            return;
        }
        if (hasCombo) //check if the attack has any combo attacks as in the combo list has more than 1 nested attacks
        {
            if (canCombo) //if the this isn't the first attack
            {
                comboDelay?.Kill();
            }
            else //if this is the first attack
            {
                canCombo = true;
            }
            OnAttackPerformed?.Invoke();
            DealDamage();
            SetCurrentComboCount();
            StartCooldownCountdowns();
            AssignAnimationData();
        }
        else //if this attack doesn't have any combo, attack once and exit attack state
        {
            OnAttackPerformed?.Invoke();
            DealDamage();
            StartCooldownCountdowns();
            AssignAnimationData();
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
        AssignAnimationData();
    }

    //Called when current attack state should end
    protected virtual void EndAttack()
    {
        canCombo = false;
        currentComboCount = 0;
        AssignAnimationData();
        OnAttackCompleted?.Invoke();
    }

    protected virtual void SetCurrentComboCount()
    {
        currentComboCount++;
        if (currentComboCount >= comboCount)
        {
            currentComboCount = 0;
        }
    }

    protected virtual void AssignAnimationData()
    {
        CurrentAttackAnimationData = attackDatas[currentComboCount].AttackAnimationData;
    }

    protected virtual void StartCooldownCountdowns()
    {
        canAttack = false;
        var info = (MeleeAttackAnimationData)CurrentAttackAnimationData;
        attackDelay = DOVirtual.DelayedCall(info.attackCD, () => //this should be slightly shorter than attack animation
        {
            canAttack = true;
            if (currentComboCount == 0)
            {
                EndAttack();
            }
        });
        if (currentComboCount != 0)
        {
            comboDelay = DOVirtual.DelayedCall(info.attackCD + 0.5f, () => //this should be as long as the attack animation
            {
                EndAttack();
            });
        }
    }

    protected virtual void DealDamage()
    {
        var animData = (MeleeAttackAnimationData)attackDatas[currentComboCount].AttackAnimationData;
        DOVirtual.DelayedCall(animData.attackDamageDelay, () =>
        {
            var damageData = (SphereAttackDamageData)attackDatas[currentComboCount].AttackDamageData;
            new SphereCastDamager(transform.root.position + transform.root.up, damageData.radius, transform.root.forward, damageData.range, damageData.damage, damageData.dmgDealerType);
        });
    }

    private void OnDrawGizmosSelected()
    {
        var damageData = (SphereAttackDamageData)attackDatas[0].AttackDamageData;
        Gizmos.DrawWireSphere(transform.root.position + transform.root.up, damageData.radius);
        Gizmos.DrawWireSphere(transform.root.position + transform.root.up + (transform.root.forward * damageData.range), damageData.radius);
    }
}
