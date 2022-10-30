using _Main.Scripts.GamePlay.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerInput : InputBase
    {
        private Camera _cam;
        private InputActions _inputActions;

        private Vector3 _movementInput;
        private Vector3 _lookInput;

        private bool _isInputEnabled;

        public bool IsAiming { get; private set; }

        private void Awake()
        {
            _cam = Camera.main;

            _inputActions = new InputActions();
            _inputActions.PlayerControls.Enable();

            SubscribeToInputActions();
        }

        private void SubscribeToInputActions()
        {
            _inputActions.PlayerControls.Roll.started += RollButtonPressed;
            _inputActions.PlayerControls.Aim.started += AimButtonPressed;
            _inputActions.PlayerControls.Aim.canceled += AimButtonReleased;
            _inputActions.PlayerControls.Attack.started += AttackButtonPressed;
            _inputActions.PlayerControls.Attack.canceled += AttackButtonReleased;
            _inputActions.PlayerControls.SwitchMeleeWeapon.performed += OnWeaponSwitchButtonPressed;
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
            OnAttackActionStarted?.Invoke();
        }

        private void AttackButtonReleased(InputAction.CallbackContext ctx)
        {
            OnAttackActionEnded?.Invoke();
        }

        private void OnWeaponSwitchButtonPressed(InputAction.CallbackContext ctx)
        {
            OnMeleeWeaponSwitched?.Invoke(ctx.ReadValue<Vector2>().y);
        }

        private void Update()
        {
            ReadMovementInput();
            ReadLookInput();
        }

        private void ReadMovementInput()
        {
            var readMovementVector = _inputActions.PlayerControls.Movement.ReadValue<Vector2>();
            _movementInput = new Vector3(readMovementVector.x, 0f, readMovementVector.y);
        }

        private void ReadLookInput()
        {
            var readLookVector = _inputActions.PlayerControls.Look.ReadValue<Vector2>();
            if (_inputActions.PlayerControls.Look.activeControl != null)
            {
                if (_inputActions.PlayerControls.Look.activeControl.device.displayName == "Mouse")
                {
                    _lookInput = GetAxisFromMousePos(readLookVector);
                }
                else
                {
                    _lookInput = new Vector3(readLookVector.x, 0f, readLookVector.y);
                }
            }
            else
            {
                _lookInput = _movementInput;
            }
        }

        private Vector3 GetAxisFromMousePos(Vector2 mousePosition)
        {
            var playerPos = _cam.WorldToScreenPoint(transform.position);
            playerPos.z = 0f;
            var dir = (Vector3)mousePosition - playerPos;
            dir.z = dir.y;
            dir.y = 0f;
            dir.Normalize();
            dir = Quaternion.Euler(0f, _cam.transform.rotation.y, 0f) * dir;
            return dir;
        }

        public bool IsInputEnabled()
        {
            return _isInputEnabled;
        }

        public override Vector3 GetMovementInput()
        {
            return _movementInput;
        }

        public override Vector3 GetLookInput()
        {
            return _lookInput;
        }

        public void ToggleInput(bool isOn)
        {
            _isInputEnabled = isOn;
        }
    }
}