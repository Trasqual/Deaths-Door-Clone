using System;
using _Main.Scripts.GamePlay.ActionSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public abstract class AttackBase : MonoBehaviour
    {
        public Action OnAttackCompleted;
        public AttackAnimationDataBase CurrentAttackAnimationData { get; protected set; }
        [SerializeField] protected AttackAnimationDataBase[] attackAnimationDatas;

        public virtual void Init(IAction action)
        {
            action.OnActionStart += DoOnActionStart;
            action.OnActionEnd += DoOnActionEnd;
            action.OnActionCanceled += DoOnActionCanceled;
        }

        protected abstract void DoOnActionStart();
        protected abstract void DoOnActionEnd();
        protected abstract void DoOnActionCanceled();
    }
}
