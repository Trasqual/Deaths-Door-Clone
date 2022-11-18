using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public abstract class AttackSelectorBase : MonoBehaviour
    {
        protected Transform _targetTransform;
        public Transform Target { get => _targetTransform; }

        protected AttackController _attackController;

        public AttackBase SelectedAttack { get; protected set; }

        protected virtual void Awake()
        {
            _attackController = GetComponent<AttackController>();
        }

        protected abstract void SelectAttack();

        protected abstract bool IsAttackAvailable(AttackBase attack);

        protected abstract bool TargetIsInAttackRange(AttackBase attack);
    }
}

