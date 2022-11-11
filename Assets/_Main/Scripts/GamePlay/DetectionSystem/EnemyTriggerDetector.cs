using _Main.Scripts.Utilities;
using UnityEngine;

public class EnemyTriggerDetector : TriggerDetectorBase<IDamageable>
{
    [SerializeField] LayerMask lineOfSightCheckMask;
    [SerializeField] DamageDealerType damageDealerType;

    protected override void OnTriggerStay(Collider other)
    {
        if (_target == null)
        {
            if (other.TryGetComponent(out IDamageable target) && InLineOfSight(target))
            {
                _target = target;
                _target.OnDeath += LoseTarget;
                Detect(_target);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            if (Vector3.Distance(transform.position, _target.GetTransform().position) > resetRange)
            {
                LoseTarget();
            }
        }
    }

    public override void Detect(IDamageable target)
    {
        if (Enums.CompareEnums(target.GetEffectedByType(), damageDealerType))
        OnTargetFound?.Invoke(target);
    }

    protected override void LoseTarget()
    {
        _target.OnDeath -= LoseTarget;
        base.LoseTarget();
    }

    private bool InLineOfSight(IDamageable target)
    {
        RaycastHit hit;
        var dir = target.GetTransform().position - transform.position;
        if (Physics.Raycast(transform.position + transform.up, dir, out hit, resetRange, lineOfSightCheckMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform != target.GetTransform())
            {
                return false;
            }
        }
        return true;
    }
}
