using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.ActionSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public abstract class AttackBase : MonoBehaviour
    {
        public Action OnAttackPerformed;
        public Action OnAttackCompleted;
        public Action<int> OnAttackLanded;

        [SerializeField] protected List<AttackData> comboDatas = new List<AttackData>();
        public AttackAnimationDataBase CurrentComboAnimationData { get; protected set; }

        public virtual void Init(IAction action)
        {
            action.OnActionStart += DoOnActionStart;
            action.OnActionEnd += DoOnActionEnd;
            action.OnActionCanceled += DoOnActionCanceled;
        }

        public virtual void Release(IAction action)
        {
            action.OnActionStart -= DoOnActionStart;
            action.OnActionEnd -= DoOnActionEnd;
            action.OnActionCanceled -= DoOnActionCanceled;
        }

        protected abstract void DoOnActionStart();
        protected abstract void DoOnActionEnd();
        protected abstract void DoOnActionCanceled();
    }
}
