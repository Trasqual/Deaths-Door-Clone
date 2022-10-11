using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.MeleeAttacks
{
    public class UnarmedAttack : AttackBase
    {
        [SerializeField] int comboCount = 2;
        [SerializeField] float comboWaitTime = 0.5f;
        float comboTimer;

        [SerializeField] ParticleSystem unarmedParticle;

        int currentComboCount;

        bool isActive;
        bool canCombo;

        protected override void DoOnActionStart()
        {
            currentComboCount++;
            CurrentAttackAnimationData = attackAnimationDatas[currentComboCount - 1];
            comboTimer = 0f;
            isActive = true;
            Debug.Log(currentComboCount);
        }

        protected override void DoOnActionEnd()
        {
            if (currentComboCount >= attackAnimationDatas.Length)
            {
                comboTimer = 0;
                currentComboCount = 0;
                canCombo = false;
                OnAttackEnded?.Invoke();
                isActive = false;
            }
        }

        protected override void DoOnActionCanceled()
        {
            currentComboCount = 0;
            isActive = false;
        }

        private void Update()
        {
            if (isActive)
            {
                canCombo = true;
                comboTimer += Time.deltaTime;
                if (comboTimer >= comboWaitTime)
                {
                    comboTimer = 0;
                    currentComboCount = 0;
                    canCombo = false;
                    OnAttackEnded?.Invoke();
                    isActive = false;
                }
            }
        }
    }
}
