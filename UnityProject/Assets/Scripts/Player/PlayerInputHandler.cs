using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 movementInput { get; protected set; }
    public Vector2 aimInput { get; protected set; }

    private PlayerInputActions playerInputActions;
    private PlayerController playerController;

    void Awake()
    {
        InitInput();
    }

    private void InitInput()
    {
        if (playerInputActions == null)
            playerInputActions = new PlayerInputActions();

        if (playerController == null)
            playerController = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        playerInputActions.Enable();

        playerInputActions.PlayerControls.Move.performed += Move_Performed;
        playerInputActions.PlayerControls.Move.canceled += Move_Cancelled;
        playerInputActions.PlayerControls.Use.performed += Use_Performed;
        playerInputActions.PlayerControls.PickUpRelease.performed += PickUpRealease_Performed;
        playerInputActions.MainMenuController.ExitGame.performed += Exit_Performed;
    }

    void OnDisable()
    {
        playerInputActions.PlayerControls.Move.performed -= Move_Performed;
        playerInputActions.PlayerControls.Move.canceled -= Move_Cancelled;
        playerInputActions.PlayerControls.Use.performed -= Use_Performed;
        playerInputActions.PlayerControls.PickUpRelease.performed -= PickUpRealease_Performed;
        playerInputActions.MainMenuController.ExitGame.performed -= Exit_Performed;

        playerInputActions.Disable();
    }

    private void Move_Performed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    private void Move_Cancelled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }

    private void PickUpRealease_Performed(InputAction.CallbackContext context)
    {
        playerController.PickUpRelease();
    }

    private void Use_Performed(InputAction.CallbackContext context)
    {
        // ToDo: Use Item
    }

    private void Exit_Performed(InputAction.CallbackContext context)
    {
        LevelManager.Instance.loadScene();
    }

}
