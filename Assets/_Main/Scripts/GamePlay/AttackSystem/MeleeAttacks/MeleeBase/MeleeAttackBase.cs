using _Main.Scripts.GamePlay.AttackSystem;
using DG.Tweening;
using UnityEngine;

public class MeleeAttackBase : AttackBase
{
    public float GeneralAttackCooldown = 0.5f;
    private bool _canAttack = true;
    private Tween _attackDelay;

    private int _comboCount => _comboDatas.Count;
    private int _currentComboCount;
    private bool _hasCombo => _comboDatas.Count > 1;
    private bool _canCombo;
    private Tween _comboDelay;
    private Tween _damageDelay;

    protected virtual void Awake()
    {
        CurrentComboAnimationData = _comboDatas[0].AttackAnimationData;
    }

    #region IActionCallbacks(FromMeleeAttackState)
    //Called when attack button is down
    protected override void DoOnActionStart() //can move everything to DoOnActionEnd for charge attacks
    {
        //if (!canAttack && !canCombo) //if the attack is on CD and can't combo exit attack state
        //{
        //    EndAttack();
        //    return;
        //}
        if (!_canAttack) //if the attack is on CD but can combo don't do anything
        {
            return;
        }
        if (_hasCombo) //check if the attack has any combo attacks as in the combo list has more than 1 nested attacks
        {
            if (_canCombo) //if the this isn't the first attack
            {
                _comboDelay?.Kill();
            }
            else //if this is the first attack
            {
                _canCombo = true;
            }
            OnAttackPerformed?.Invoke();
            DealDamage();
            SetCurrentComboCount();
            StartCountdowns();
            AssignComboAnimationData();
        }
        else //if this attack doesn't have any combo, attack once and exit attack state
        {
            OnAttackPerformed?.Invoke();
            DealDamage();
            StartCountdowns();
            AssignComboAnimationData();
        }
    }

    //Called when attack button is up
    protected override void DoOnActionEnd()
    {

    }

    //Called when attack state is canceled
    protected override void DoOnActionCanceled()
    {
        _comboDelay?.Kill();
        _attackDelay?.Kill();
        _damageDelay?.Kill();
        _canAttack = true;
        _canCombo = false;
        _currentComboCount = 0;
        AssignComboAnimationData();
    }
    #endregion

    //Set which Combo should be used next
    protected virtual void SetCurrentComboCount()
    {
        _currentComboCount++;
        if (_currentComboCount >= _comboCount)
        {
            _currentComboCount = 0;
        }
    }

    //Setup Current Combo's animation data
    protected virtual void AssignComboAnimationData()
    {
        CurrentComboAnimationData = _comboDatas[_currentComboCount].AttackAnimationData;
    }

    //Setup cooldown timer for next combo attack and countdown timer for possible combo duration
    protected virtual void StartCountdowns()
    {
        _canAttack = false;
        var info = (MeleeAttackAnimationData)CurrentComboAnimationData;
        _attackDelay = DOVirtual.DelayedCall(info.attackCD, () => //this should be slightly shorter than attack animation
        {
            _canAttack = true;
            if (_currentComboCount == 0)
            {
                EndAttack();
            }
        });
        if (_currentComboCount != 0)
        {
            _comboDelay = DOVirtual.DelayedCall(info.attackCD + 0.5f, () => //this should be as long as the attack animation
            {
                EndAttack();
            });
        }
    }

    protected virtual void DealDamage()
    {
        var animData = (MeleeAttackAnimationData)_comboDatas[_currentComboCount].AttackAnimationData;
        _damageDelay = DOVirtual.DelayedCall(animData.attackDamageDelay, () =>
        {
            var damageData = (SphereAttackDamageData)_comboDatas[_currentComboCount].AttackDamageData;
            new SphereCastDamager(transform.root.position + transform.root.up + transform.root.forward, damageData.radius, transform.root.forward, damageData.range, damageData.damage, damageData.dmgDealerType);
        });
    }

    //Called when current attack state should end
    protected virtual void EndAttack()
    {
        _comboDelay?.Kill();
        _attackDelay?.Kill();
        _canCombo = false;
        _currentComboCount = 0;
        AssignComboAnimationData();
        OnAttackCompleted?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        var damageData = (SphereAttackDamageData)_comboDatas[0].AttackDamageData;
        Gizmos.DrawWireSphere(transform.root.position + transform.root.up + transform.root.forward, damageData.radius);
        Gizmos.DrawWireSphere(transform.root.position + transform.root.up + (transform.root.forward * damageData.range), damageData.radius);
    }
}
