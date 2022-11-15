using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.GamePlay.PuzzleSystem
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] List<LaserColumn> _laserColumns = new List<LaserColumn>();
        [SerializeField] List<LockPiece> _lockPieces = new List<LockPiece>();
        [SerializeField] UnlockableDoor _unlockableDoor;

        private void Start()
        {
            _unlockableDoor.OnDoorOpened += StopLazers;

            SetupPuzzle();
        }

        private void SetupPuzzle()
        {
            _unlockableDoor.Initialize(_lockPieces);
        }

        private void StopLazers()
        {
            foreach (var laserColumn in _laserColumns)
            {
                laserColumn.DeActivate();
            }
        }
    }
}