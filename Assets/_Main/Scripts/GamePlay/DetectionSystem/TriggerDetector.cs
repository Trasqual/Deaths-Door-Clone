using UnityEngine;

public class TriggerDetectorBase<T> : DetectorBase<T>
{
    [SerializeField] protected float range = 8f;
    [SerializeField] protected float resetRange = 15f;

    protected T _target;

    protected virtual void Awake()
    {
        GetComponent<SphereCollider>().radius = range;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_target == null)
        {
            if (other.TryGetComponent(out T target))
            {
                _target = target;
                OnTargetFound?.Invoke(_target);
            }
        }
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
