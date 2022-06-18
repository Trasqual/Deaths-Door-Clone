using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    [RequireComponent(typeof(PlayerMovement),
        typeof(PlayerAnimation))]
    public class Player : MonoBehaviour
    {
        public PlayerData Data { get; private set; }
        public InputBase Input { get; private set; }
        public CharacterController Controller { get; private set; }
        public AnimationBase PlayerAnim { get; private set; }

        private StateMachine stateMachine;
        private RangedAttack selectedRangedAttack;
        private CinemachineTargetGroupHandler targetGroupHandler;
        private AimActionIndicator aimIndicatior;

        private void Awake()
        {
            //Data = GetComponent<PlayerData>();
            Input = GetComponent<InputBase>();
            Controller = GetComponent<CharacterController>();
            PlayerAnim = GetComponent<AnimationBase>();
            stateMachine = GetComponent<StateMachine>();
            selectedRangedAttack = GetComponentInChildren<RangedAttack>();
            targetGroupHandler = GetComponentInChildren<CinemachineTargetGroupHandler>();
            aimIndicatior = GetComponentInChildren<AimActionIndicator>();

            Input.OnAimActionStarted += StartAiming;
            Input.OnAimActionEnded += EndAiming;
            Input.OnRollAction += PerformRoll;
        }

        private void Start()
        {
            SetSelectedRangedAttack();
            targetGroupHandler.Init(stateMachine.AimingState);
            aimIndicatior.Init(stateMachine.AimingState);
        }

        private void StartAiming()
        {
            stateMachine.ChangeState(stateMachine.AimingState);
        }

        private void EndAiming()
        {
            stateMachine.AimingState.EndAim();
        }

        private void PerformRoll()
        {
            stateMachine.ChangeState(stateMachine.RollingState);
        }

        private void SetSelectedRangedAttack()
        {
            selectedRangedAttack.Init(stateMachine.AimingState);
        }
    }
}