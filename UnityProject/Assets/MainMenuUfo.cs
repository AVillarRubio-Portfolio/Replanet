using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuUfo : MonoBehaviour
{
    public LevelLoader levelLoader;
    private Animator animator;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (playerInputActions == null)
            playerInputActions = new PlayerInputActions();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        playerInputActions.Enable();

        playerInputActions.MainMenuController.StartGame.performed += StartGameAnim;
        playerInputActions.MainMenuController.ExitGame.performed += ExitGameAnim;
    }

    void OnDisable()
    {
        playerInputActions.Disable();

        playerInputActions.MainMenuController.StartGame.performed -= StartGameAnim;
        playerInputActions.MainMenuController.ExitGame.performed -= ExitGameAnim;
    }

    private void StartGameAnim(InputAction.CallbackContext context)
    {
        StartCoroutine(StartGameAnimation());
    }
    private void ExitGameAnim(InputAction.CallbackContext context)
    {
        StartCoroutine(ExitAnimation());
    }
    IEnumerator StartGameAnimation()
    {
        GetComponent<FMODUnity.StudioEventEmitter>().Play();
        animator.SetTrigger("Leave");
        yield return new WaitForSeconds(2f);
        StopAllCoroutines();
        levelLoader.LoadNextLevel();
    }

    IEnumerator ExitAnimation()
    {
        animator.SetTrigger("Exit");
        yield return new WaitForSeconds(2.5f);
        levelLoader.exitLevel();
    }
}
