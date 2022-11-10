using DG.Tweening;
using UnityEngine;

public class RangedEnemyInput : EnemyInputBase
{
    RangedEnemyBehaviourData enemyBehaviourData;
    private bool isAiming;

    protected override void Awake()
    {
        base.Awake();
        var data = GetComponent<EnemyBehaviourBase>().Data;
        enemyBehaviourData = (RangedEnemyBehaviourData)data;
    }

    protected override void Update()
    {
        if (_target != null)
        {
            if (Vector3.Distance(transform.position, _target.GetTransform().position) <= agent.stoppingDistance && !isAiming)
            {
                isAiming = true;
                OnAimActionStarted?.Invoke();
                DOVirtual.DelayedCall(enemyBehaviourData.CastSpeed, () =>
                {
                    OnAimActionEnded?.Invoke();
                    isAiming = false;
                });
            }
        }
    }
}
