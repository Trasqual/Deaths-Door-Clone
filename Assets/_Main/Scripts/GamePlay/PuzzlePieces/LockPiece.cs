using _Main.Scripts.Utilities;
using DG.Tweening;
using System;
using UnityEngine;

public class LockPiece : MonoBehaviour, IDamagable
{
    public Action<LockPiece> OnLockPieceBroken;

    [SerializeField] DamageDealerType _effectedByType;

    private bool _isUnlocked;

    public void TakeDamage(int amount, DamageDealerType damageDealerType)
    {
        if (Enums.CompareEnums(damageDealerType, _effectedByType))
        {
            BreakLock();
        }
    }

    private void BreakLock()
    {
        transform.DOShakeScale(0.5f, 0.2f, 30, 60).OnComplete(() => transform.localScale = Vector3.one);
        if (_isUnlocked) return;
        _isUnlocked = true;
        OnLockPieceBroken?.Invoke(this);
    }
}
