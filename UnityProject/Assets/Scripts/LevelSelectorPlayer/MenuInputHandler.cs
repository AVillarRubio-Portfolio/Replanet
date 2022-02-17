using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputHandler : MonoBehaviour
{
    public Vector2 movementInput { get; protected set; }
    public Vector2 aimInput { get; protected set; }

    private PlayerInputActions playerInputActions;
    private MenuPlayerController menuPlayerController;

    void Awake()
    {
        InitInput();
    }

    private void InitInput()
    {
        if (playerInputActions == null)
            playerInputActions = new PlayerInputActions();

        if (menuPlayerController == null)
            menuPlayerController = GetComponent<MenuPlayerController>();
    }

    void OnEnable()
    {
        playerInputActions.Enable();

        playerInputActions.MenuPlayerControls.Move.performed += Move_Performed;
        playerInputActions.MenuPlayerControls.Move.canceled += Move_Cancelled;
        playerInputActions.MenuPlayerControls.Use.performed += Use_Performed; 
        playerInputActions.MainMenuController.ExitGame.performed += Exit_Performed;
    }

    void OnDisable()
    {
        playerInputActions.PlayerControls.Move.performed -= Move_Performed;
        playerInputActions.PlayerControls.Move.canceled -= Move_Cancelled;
        playerInputActions.PlayerControls.Use.performed -= Use_Performed;
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

    private void Use_Performed(InputAction.CallbackContext context)
    {
        menuPlayerController.chooseLevel();
    }

    private void Exit_Performed(InputAction.CallbackContext context)
    {
        menuPlayerController.Exit();
    }
}
