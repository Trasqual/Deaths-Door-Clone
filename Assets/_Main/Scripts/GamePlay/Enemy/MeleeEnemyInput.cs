using UnityEngine;

public class MeleeEnemyInput : EnemyInputBase
{
    protected override void Update()
    {
        if (_target != null)
        {
            if (Vector3.Distance(transform.position, _target.GetTransform().position) <= agent.stoppingDistance)
            {
                OnAttackActionStarted?.Invoke();
            }
        }
    }
}
