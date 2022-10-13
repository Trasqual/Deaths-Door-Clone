using _Main.Scripts.GamePlay.AttackSystem;
using DG.Tweening;

public class MeleeAttackBase : AttackBase
{
    bool canAttack = true;
    Tween attackDelay;

    int comboCount => attackDatas.Count;
    int currentComboCount;
    bool hasCombo => attackDatas.Count > 1;
    bool canCombo;
    Tween comboDelay;

    private void Awake()
    {
        CurrentAttackAnimationData = attackDatas[0].AttackAnimationData;
    }

    //Called when attack button is down
    protected override void DoOnActionStart()
    {
        if (!canAttack && !canCombo) //if the attack is on CD exit attack state
        {
            EndAttack();
            return;
        }
        else if (!canAttack)
        {
            return;
        }
        if (hasCombo)
        {
            if (canCombo)
            {
                comboDelay?.Kill();
            }
            else
            {
                canCombo = true;
            }
            OnAttackPerformed?.Invoke();
            SetCurrentComboCount();
            StartCooldownCountDowns();
            AssignAnimationData();
            var damageData = (SphereAttackDamageData)attackDatas[currentComboCount].AttackDamageData;
            new SphereCastDamager(transform.position, damageData.radius, transform.forward, damageData.range, damageData.damage, damageData.dmgDealerType);
        }
        else //if this attack doesn't have combo attack once and exit attack state
        {
            OnAttackPerformed?.Invoke();
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
        AssignAnimationData();
    }

    //Called when final attack is used
    private void EndAttack()
    {
        canCombo = false;
        currentComboCount = 0;
        AssignAnimationData();
        OnAttackCompleted?.Invoke();
    }

    private void SetCurrentComboCount()
    {
        currentComboCount++;
        if (currentComboCount >= comboCount)
        {
            currentComboCount = 0;
        }
    }

    private void AssignAnimationData()
    {
        CurrentAttackAnimationData = attackDatas[currentComboCount].AttackAnimationData;
    }

    private void StartCooldownCountDowns()
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
}
