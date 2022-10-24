using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Utilities;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public Action<IDamagable> OnTargetFound;
    public Action OnTargetLost;

    [SerializeField] DamageDealerType triggerType; //should be the same as behaviourbase type
    [SerializeField] float range = 8f;
    [SerializeField] float resetRange = 15f;

    IDamagable target;

    private void Awake()
    {
        GetComponent<SphereCollider>().radius = range;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target != null) return;

        if (other.TryGetComponent(out IDamagable damagable))
        {
            if (Enums.CompareEnums(triggerType, damagable.GetEffectedByType()))
            {
                target = damagable;
                OnTargetFound?.Invoke(target);
            }
        }
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            if(Vector3.Distance(transform.position, target.GetTransform().transform.position) > resetRange)
            {
                target = null;
                OnTargetLost?.Invoke();
            }
        }
    }
}
