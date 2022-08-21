using DG.Tweening;
using System;
using UnityEngine;

public class LockPiece : MonoBehaviour, IDamagable
{
    public Action<LockPiece> OnLockPieceBroken;

    public void TakeDamage(int amount, DamageDealerType damageDealerType)
    {
        if (damageDealerType == DamageDealerType.Player)
        {
            BreakLock();
        }
    }

    private void BreakLock()
    {
        transform.DOShakeScale(0.5f, 0.2f, 30, 60).OnComplete(() => transform.localScale = Vector3.one);
        OnLockPieceBroken?.Invoke(this);
    }
}
