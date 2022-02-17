using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 6.0f;

   

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void exitLevel()
    {
        StartCoroutine("ExitLevel");
    }

    IEnumerator ExitLevel()
    {
        transition.SetTrigger("Exit");

        yield return new WaitForSeconds(transitionTime + 0.5f);

        Application.Quit();
        Debug.Log("Adios");
    }
}

