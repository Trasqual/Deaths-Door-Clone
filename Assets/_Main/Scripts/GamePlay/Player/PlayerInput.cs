using _Main.Scripts.GamePlay.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerInput : InputBase
    {
        private Camera cam;
        private InputActions inputActions;

        private Vector3 movementInput;
        private Vector3 lookInput;

        private bool isInputEnabled;

        public bool IsAiming { get; private set; }

        private void Awake()
        {
            cam = Camera.main;

            inputActions = new InputActions();
            inputActions.PlayerControls.Enable();

            SubscribeToInputActions();
        }

        private void SubscribeToInputActions()
        {
            inputActions.PlayerControls.Roll.started += RollButtonPressed;
            inputActions.PlayerControls.Aim.started += AimButtonPressed;
            inputActions.PlayerControls.Aim.canceled += AimButtonReleased;
            inputActions.PlayerControls.Attack.started += AttackButtonPressed;
        }

        private void RollButtonPressed(InputAction.CallbackContext ctx)
        {
            OnRollAction?.Invoke();
        }

        private void AimButtonPressed(InputAction.CallbackContext ctx)
        {
            if (IsAiming) return;
            IsAiming = true;
            OnAimActionStarted?.Invoke();
        }

        private void AimButtonReleased(InputAction.CallbackContext ctx)
        {
            if (!IsAiming) return;
            IsAiming = false;
            OnAimActionEnded?.Invoke();
        }

        private void AttackButtonPressed(InputAction.CallbackContext ctx)
        {
            OnAttackAction?.Invoke();
        }

        private void Update()
        {
            ReadMovementInput();
            ReadLookInput();
        }

        private void ReadMovementInput()
        {
            var readMovementVector = inputActions.PlayerControls.Movement.ReadValue<Vector2>();
            movementInput = new Vector3(readMovementVector.x, 0f, readMovementVector.y);
        }

        private void ReadLookInput()
        {
            var readLookVector = inputActions.PlayerControls.Look.ReadValue<Vector2>();

            if (inputActions.PlayerControls.Look.activeControl != null)
            {
                if (inputActions.PlayerControls.Look.activeControl.device.displayName == "Mouse")
                {
                    lookInput = GetAxisFromMousePos(readLookVector);
                }
                else
                {
                    lookInput = new Vector3(readLookVector.x, 0f, readLookVector.y);
                }
            }
            else
            {
                lookInput = movementInput;
            }
        }

        private Vector3 GetAxisFromMousePos(Vector2 mousePosition)
        {
            var playerPos = cam.WorldToScreenPoint(transform.position);
            playerPos.z = 0f;
            var dir = (Vector3)mousePosition - playerPos;
            dir.z = dir.y;
            dir.y = 0f;
            dir.Normalize();
            dir = Quaternion.Euler(0f, cam.transform.rotation.y, 0f) * dir;
            return dir;
        }

        public bool IsInputEnabled()
        {
            return isInputEnabled;
        }

        public override Vector3 GetMovementInput()
        {
            return movementInput;
        }

        public override Vector3 GetLookInput()
        {
            return lookInput;
        }

        public void ToggleInput(bool isOn)
        {
            isInputEnabled = isOn;
        }
    }
}