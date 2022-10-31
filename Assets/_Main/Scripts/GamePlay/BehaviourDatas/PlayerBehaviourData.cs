using _Main.Scripts.GamePlay.Indicators.AimingIndicator;
using _Main.Scripts.Others;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    [CreateAssetMenu]
    public class PlayerBehaviourData : BehaviourData
    {
        public AimActionIndicator aimingIndicator;
        public CameraTargetGroup cameraTargetGroup;
    }
}