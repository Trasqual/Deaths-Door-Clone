using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{
    public Action OnDoorOpened;

    [SerializeField] Transform _doorRight;
    [SerializeField] Transform _doorLeft;

    [SerializeField] List<Renderer> indicators = new List<Renderer>();

    Dictionary<LockPiece, Renderer> lockPiecesAndIndicators = new Dictionary<LockPiece, Renderer>();

    List<LockPiece> brokenLockPieces = new List<LockPiece>();

    public void Initialize(List<LockPiece> lockPieces)
    {
        if (lockPieces.Count != indicators.Count)
        {
            Debug.Log("Indicator count doesn't match lock piece count. Control the lists.");
        }
        else
        {
            for (int i = 0; i < lockPieces.Count; i++)
            {
                lockPiecesAndIndicators[lockPieces[i]] = indicators[i];
                lockPieces[i].OnLockPieceBroken += ProcessBrokenLockPiece;
            }
        }
    }

    private void ProcessBrokenLockPiece(LockPiece lockPiece)
    {
        brokenLockPieces.Add(lockPiece);
        lockPiecesAndIndicators[lockPiece].material.SetColor("_Color", Color.green);
        lockPiece.OnLockPieceBroken -= ProcessBrokenLockPiece;
        if (brokenLockPieces.Count >= indicators.Count)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        _doorRight.DORotate(new Vector3(0f, 90f, 0f), 2f).SetEase(Ease.InCubic);
        _doorLeft.DORotate(new Vector3(0f, -90f, 0f), 2f).SetEase(Ease.InCubic);
        OnDoorOpened?.Invoke();
    }
}
