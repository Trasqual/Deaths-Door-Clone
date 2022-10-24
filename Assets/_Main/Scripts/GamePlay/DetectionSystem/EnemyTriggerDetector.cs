using _Main.Scripts.GamePlay.Player;
using UnityEngine;

public class EnemyTriggerDetector : TriggerDetectorBase<Player>
{
    private void FixedUpdate()
    {
        if(_target != null)
        {
            if ( Vector3.Distance(transform.position, _target.transform.position) > resetRange || !InLineOfSight())
            {
                LoseTarget();
            }
        }
    }

    private bool InLineOfSight()
    {
        RaycastHit hit;
        var dir = _target.transform.position - transform.position;
        if(Physics.Raycast(transform.position+transform.up, dir, out hit, resetRange))
        {
            if(hit.transform != _target.transform)
            {
                return false;
            }
        }
        return true;
    }
}
