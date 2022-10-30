using _Main.Scripts.GamePlay.Player;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{
    public Action OnDoorOpened;

    [SerializeField] private Animator _anim;

    [SerializeField] private List<Renderer> _indicators = new List<Renderer>();

    private Dictionary<LockPiece, Renderer> _lockPiecesAndIndicators = new Dictionary<LockPiece, Renderer>();

    private List<LockPiece> _brokenLockPieces = new List<LockPiece>();

    private bool _isClosed;

    public void Initialize(List<LockPiece> lockPieces)
    {
        if (lockPieces.Count != _indicators.Count)
        {
            Debug.Log("Indicator count doesn't match lock piece count. Control the lists.");
        }
        else
        {
            for (int i = 0; i < lockPieces.Count; i++)
            {
                _lockPiecesAndIndicators[lockPieces[i]] = _indicators[i];
                lockPieces[i].OnLockPieceBroken += ProcessBrokenLockPiece;
            }
        }
    }

    private void ProcessBrokenLockPiece(LockPiece lockPiece)
    {
        if (_brokenLockPieces.Contains(lockPiece)) return;
        _brokenLockPieces.Add(lockPiece);
        _lockPiecesAndIndicators[lockPiece].material.SetColor("_Color", Color.green);
        lockPiece.OnLockPieceBroken -= ProcessBrokenLockPiece;
        if (_brokenLockPieces.Count >= _indicators.Count)
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
        if (_isClosed) return;
        if (other.TryGetComponent(out Player player))
        {
            CloseDoor();
        }
    }

    private void CloseDoor()
    {
        _anim.SetBool("character_nearby", false);
    }
}
