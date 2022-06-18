using System;
using System.Collections.Generic;
using System.Linq;
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
        private CinemachineTargetGroupHandler targetGroupHandler;
        private AimActionIndicator aimIndicatior;

        private List<RangedAttack> rangedAttacks = new List<RangedAttack>();
        private RangedAttack selectedRangedAttack;

        private void Awake()
        {
            //Data = GetComponent<PlayerData>();
            Input = GetComponent<InputBase>();
            Controller = GetComponent<CharacterController>();
            PlayerAnim = GetComponent<AnimationBase>();
            stateMachine = GetComponent<StateMachine>();
            rangedAttacks = GetComponentsInChildren<RangedAttack>().ToList();
            targetGroupHandler = GetComponentInChildren<CinemachineTargetGroupHandler>();
            aimIndicatior = GetComponentInChildren<AimActionIndicator>();

            Input.OnAimActionStarted += StartAiming;
            Input.OnAimActionEnded += EndAiming;
            Input.OnRollAction += PerformRoll;
        }

        private void Start()
        {
            SetSelectedRangedAttack(typeof(BowAttack));
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

        private void SetSelectedRangedAttack(Type rangedAttackType)
        {
            foreach (var rangedAttack in rangedAttacks)
            {
                if (rangedAttack.GetType() == rangedAttackType)
                {
                    selectedRangedAttack = rangedAttack;
                }
            }
            selectedRangedAttack.Init(stateMachine.AimingState);
        }
    }
}