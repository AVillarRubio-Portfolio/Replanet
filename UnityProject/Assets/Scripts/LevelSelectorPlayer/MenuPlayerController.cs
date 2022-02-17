using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayerController : MonoBehaviour
{
    [SerializeField] private float aimSensitivity = 10f;
    [Range(0f, 1f)] [SerializeField] private float buildUpRot;
    [SerializeField] private float minInput;    

    // Aim
    private Vector3 aimTarget;
    private Vector3 forward;
    private Vector3 right;

    private MenuInputHandler playerInput;    
    

    #region Gets
    public float GetMinInput() { return minInput; }
    public float GetBuildUpRot() { return buildUpRot; }

    private string levelToChange = null;
    private int levelToChangeIdx = -1;

    public MessageFade message;
    
    #endregion

    void Awake()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        playerInput = GetComponent<MenuInputHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LevelSelector result))
        {
            levelToChange = result.getLevel();
            levelToChangeIdx = result.getLevelIdx();
            if (levelToChangeIdx != -1)
            {
                message.FadeIn();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<LevelSelector>() != null)
        {
            levelToChange = null;
            message.FadeOut();
        }
    }    

    public void chooseLevel()
    {
        if(levelToChange != null)
        {
            Debug.Log("Cambio al nivel: " + levelToChange);
            Globals.actualLevel = levelToChangeIdx + 1;
            LevelSelectorManager.instance.loadScene(levelToChange);
        }
        else
        {
            Debug.Log("levelToChange es null");
        }
    }

    public void Exit()
    {
        LevelSelectorManager.instance.exitToMenu();
    }
}
