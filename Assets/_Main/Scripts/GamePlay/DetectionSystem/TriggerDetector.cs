using UnityEngine;

public class TriggerDetectorBase<T> : DetectorBase<T>
{
    [SerializeField] protected float _range = 8f;
    [SerializeField] protected float _resetRange = 15f;

    protected T _target;

    protected virtual void Awake()
    {
        GetComponent<SphereCollider>().radius = _range;
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
        Debug.Log(target);
    }

    protected virtual void LoseTarget()
    {
        OnTargetLost?.Invoke();
        _target = default;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        GetComponent<SphereCollider>().radius = _range;
    }
#endif
}
