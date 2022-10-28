using _Main.Scripts.GamePlay.Player;
using UnityEngine;

public class EnemyTriggerDetector : TriggerDetectorBase<Player>
{
    [SerializeField] LayerMask lineOfSightCheckMask;
    protected override void OnTriggerStay(Collider other)
    {
        if (_target == null)
        {
            if (other.TryGetComponent(out Player target) && InLineOfSight(target))
            {
                _target = target;
                OnTargetFound?.Invoke(_target);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            if (Vector3.Distance(transform.position, _target.transform.position) > resetRange)
            {
                LoseTarget();
            }
        }
    }

    private bool InLineOfSight(Player target)
    {
        RaycastHit hit;
        var dir = target.transform.position - transform.position;
        if (Physics.Raycast(transform.position + transform.up, dir, out hit, resetRange, lineOfSightCheckMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform != target.transform)
            {
                return false;
            }
        }
        return true;
    }
}
