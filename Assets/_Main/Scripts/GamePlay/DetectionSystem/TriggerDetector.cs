using UnityEngine;

namespace _Main.Scripts.GamePlay.DetectionSystem
{
    public class TriggerDetectorBase<T> : DetectorBase<T>
    {
        [SerializeField] protected float range = 8f;
        [SerializeField] protected float resetRange = 15f;

        protected T _target;

        protected virtual void Awake()
        {
            GetComponent<SphereCollider>().radius = range;
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (_target == null)
            {
                if (other.TryGetComponent(out T target))
                {
                    _target = target;
                    Detect(_target);
                }
            }
        }

        public override void Detect(T target)
        {
            OnTargetFound?.Invoke(target);
        }

        protected virtual void LoseTarget()
        {
            OnTargetLost?.Invoke();
            _target = default;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            GetComponent<SphereCollider>().radius = range;
        }
#endif
    }
}