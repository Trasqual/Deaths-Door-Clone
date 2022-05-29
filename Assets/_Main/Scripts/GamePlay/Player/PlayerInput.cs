using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Action OnRollButtonPressed;

    Camera cam;
    InputActions inputActions;

    Vector3 movementInput;
    Vector3 lookInput;

    bool isInputEnabled;


    private void Awake()
    {
        cam = Camera.main;

        inputActions = new InputActions();
        inputActions.PlayerControls.Enable();

        SubscribeToInputActions();
    }

    private void SubscribeToInputActions()
    {
        inputActions.PlayerControls.Roll.performed += RollButtonPressed;
    }

    private void RollButtonPressed(InputAction.CallbackContext ctx)
    {
        OnRollButtonPressed?.Invoke();
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

        lookInput = new Vector3(readLookVector.x, 0f, readLookVector.y);
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

    public Vector3 GetMovementInput()
    {
        return movementInput;
    }

    public Vector3 GetLookInput()
    {
        return lookInput;
    }

    public void ToggleInput(bool isOn)
    {
        isInputEnabled = isOn;
    }
}