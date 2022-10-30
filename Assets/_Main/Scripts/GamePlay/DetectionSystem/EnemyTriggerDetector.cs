using UnityEngine;

public class EnemyTriggerDetector : TriggerDetectorBase<IDamageable>
{
    [SerializeField] LayerMask lineOfSightCheckMask;
    protected override void OnTriggerStay(Collider other)
    {
        if (_target == null)
        {
            if (other.TryGetComponent(out IDamageable target) && InLineOfSight(target))
            {
                _target = target;
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
