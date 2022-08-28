using _Main.Scripts.GamePlay.Player;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{
    public Action OnDoorOpened;

    [SerializeField] Animator _anim;

    [SerializeField] List<Renderer> indicators = new List<Renderer>();

    Dictionary<LockPiece, Renderer> lockPiecesAndIndicators = new Dictionary<LockPiece, Renderer>();

    List<LockPiece> brokenLockPieces = new List<LockPiece>();

    bool isClosed;

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
        _anim.SetBool("character_nearby", true);
        OnDoorOpened?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (isClosed) return;
        if(other.TryGetComponent(out Player player))
        {
            CloseDoor();
        }
    }

    private  void CloseDoor()
    {
        _anim.SetBool("character_nearby", false);
    }
}
