using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class MeleeAttackBase : AttackBase
    {
        bool canAttack = true;
        Tween attackDelay;

        int comboCount => comboDatas.Count;
        int currentComboCount;
        bool hasCombo => comboDatas.Count > 1;
        bool canCombo;
        Tween comboDelay;
        Tween damageDelay;

        protected virtual void Awake()
        {
            CurrentComboAnimationData = comboDatas[0].AttackAnimationData;
            CurrentComboDamageData = comboDatas[0].AttackDamageData;
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
                StartCountdowns();
                AssignComboData();
            }
            else //if this attack doesn't have any combo, attack once and exit attack state
            {
                OnAttackPerformed?.Invoke();
                DealDamage();
                StartCountdowns();
                AssignComboData();
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
            damageDelay?.Kill();
            canAttack = true;
            canCombo = false;
            currentComboCount = 0;
            AssignComboData();
        }
        #endregion

        //Set which Combo should be used next
        protected virtual void SetCurrentComboCount()
        {
            currentComboCount++;
            if (currentComboCount >= comboCount)
            {
                currentComboCount = 0;
            }
        }

        //Setup Current Combo's animation data
        protected virtual void AssignComboData()
        {
            CurrentComboAnimationData = comboDatas[currentComboCount].AttackAnimationData;
            CurrentComboDamageData = comboDatas[currentComboCount].AttackDamageData;
        }

        //Setup cooldown timer for next combo attack and countdown timer for possible combo duration
        protected virtual void StartCountdowns()
        {
            canAttack = false;
            var info = (MeleeAttackAnimationData)CurrentComboAnimationData;
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
                comboDelay = DOVirtual.DelayedCall(info.attackCD + 0.2f, () => //this should be as long as the attack animation
                {
                    EndAttack();
                });
            }
        }

        protected virtual void DealDamage()
        {
            var animData = (MeleeAttackAnimationData)comboDatas[currentComboCount].AttackAnimationData;
            damageDelay = DOVirtual.DelayedCall(animData.attackDamageDelay, () =>
            {
                var damageData = (SphereAttackDamageData)CurrentComboDamageData;
                damageData.DealDamage(transform.root, out int damagedTargets);
                OnAttackLanded?.Invoke(damagedTargets);
            });
        }

        //Called when current attack state should end
        protected virtual void EndAttack()
        {
            comboDelay?.Kill();
            attackDelay?.Kill();
            canCombo = false;
            currentComboCount = 0;
            AssignComboData();
            OnAttackCompleted?.Invoke();
            StartCD();
        }

        public void StartCD()
        {
            if (!IsOnCooldown)
            {
                IsOnCooldown = true;
                DOVirtual.DelayedCall(GeneralAttackCooldown, () => IsOnCooldown = false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            var damageData = (SphereAttackDamageData)comboDatas[0].AttackDamageData;
            Gizmos.DrawWireSphere(transform.root.position + transform.root.up + transform.root.forward, damageData.radius);
            Gizmos.DrawWireSphere(transform.root.position + transform.root.up + transform.root.forward * damageData.range, damageData.radius);
        }
    }
}