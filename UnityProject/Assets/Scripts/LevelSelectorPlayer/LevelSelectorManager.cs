using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Globals
{
    public static int actualLevel = 0;
    public static int maxLevel = 0;
    public static bool lastLevelDone = false;
    
}

public class LevelSelectorManager : MonoBehaviour
{
    public static LevelSelectorManager instance;

    public int numNiveles = 7;

    int maxUnlockedIndex;

    public SmoothCamera camera;
    public MenuPlayerMovement menuPlayerMovement;
    public LevelSelectorCircleInOut inOut;
    public CircleFadeOut fadeOutScript;

    private string sceneToChange = "";

    public Vector3 offset;

    // Para la colocación del player tras terminar el nivel
    public LevelSelector[] originalPositions;

    // Para la actualizacion del resto
    public LevelSelector[] restPositions0;
    public LevelSelector[] restPositions1;
    public LevelSelector[] restPositions2;
    public LevelSelector[] restPositions3;
    public LevelSelector[] restPositions4;
    public LevelSelector[] restPositions5;
    public LevelSelector[] restPositions6;

    List<LevelSelector[]> restPositions;

    private bool exiting = false;    

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        restPositions = new List<LevelSelector[]>();

        restPositions.Insert(0, restPositions6);
        restPositions.Insert(0, restPositions5);
        restPositions.Insert(0, restPositions4);
        restPositions.Insert(0, restPositions3);
        restPositions.Insert(0, restPositions2);
        restPositions.Insert(0, restPositions1);
        restPositions.Insert(0, restPositions0);

        maxUnlockedIndex = Globals.maxLevel;

        if (Globals.lastLevelDone && Globals.actualLevel > maxUnlockedIndex)
        {
            menuPlayerMovement.setActive(false);
            Globals.maxLevel = Globals.actualLevel;
            StartCoroutine("unlockNextLevel");
        }
        else
        {
            menuPlayerMovement.setActive(true);
        }

        if (maxUnlockedIndex >= originalPositions.Length)
            maxUnlockedIndex = originalPositions.Length - 1;

        menuPlayerMovement.transform.position = originalPositions[maxUnlockedIndex].transform.position + offset;

        for (int i = 0; i < maxUnlockedIndex; i++)
        {
            originalPositions[i].setState(levelState.done);

            for (int j = 0; j< restPositions0.Length; j++)
            {
                restPositions[i][j].setState(levelState.done);
            }
        }

        originalPositions[maxUnlockedIndex].setState(levelState.available);
        for (int j = 0; j < restPositions0.Length; j++)
        {
            restPositions[maxUnlockedIndex][j].setState(levelState.available);
        }

        for (int i = maxUnlockedIndex + 1; i < originalPositions.Length; i++)
        {
            originalPositions[i].setState(levelState.blocked);
            for (int j = 0; j < restPositions0.Length; j++)
            {
                restPositions[i][j].setState(levelState.blocked);
            }
        }
    }

    IEnumerator unlockNextLevel()
    {
        float smooth = camera.getSpeed();
        camera.setSpeed(0.1f);

        bool credits = false;

        yield return new WaitForSeconds(1f);
        
        camera.setTarget(originalPositions[maxUnlockedIndex].transform, false);

        yield return new WaitForSeconds(1f);

        originalPositions[maxUnlockedIndex].setState(levelState.done);
        for (int j = 0; j < restPositions0.Length; j++)
        {
            restPositions[maxUnlockedIndex][j].setState(levelState.done);
        }
        originalPositions[maxUnlockedIndex].activeParticles(particles.green);

        if (maxUnlockedIndex < originalPositions.Length - 1)
        {
            yield return new WaitForSeconds(1f);
            
            camera.setTarget(originalPositions[maxUnlockedIndex + 1].transform, false);

            yield return new WaitForSeconds(1f);

            originalPositions[maxUnlockedIndex + 1].setState(levelState.available);
            for (int j = 0; j < restPositions0.Length; j++)
            {
                restPositions[maxUnlockedIndex + 1][j].setState(levelState.available);
            }
            originalPositions[maxUnlockedIndex + 1].activeParticles(particles.red);
        }
        else
        {
            credits = true;
        }

        yield return new WaitForSeconds(1f);

        camera.setTarget(menuPlayerMovement.transform, false);

        yield return new WaitForSeconds(0.85f);

        if (credits)
        {
            yield return new WaitForSeconds(0.5f);
            exitToCredits();
        }
        else
        {
            Globals.lastLevelDone = false;
            menuPlayerMovement.setActive(true);
            camera.setLooking();
            camera.setSpeed(smooth);
        }
    }

    public void loadScene(string name)
    {
        sceneToChange = name;
        menuPlayerMovement.land();
        activePlayer(false);
        camera.setLooking(false);
        camera.setActive(false);
    }

    public void sceneOut()
    {
        //AQUI LLAMO AL METODO QUE HACE EL FADE OUT 
        //inOut.sceneOut();
        fadeOutScript.exit();
    }

    public void changeScene()
    {
        SceneManager.LoadScene(sceneToChange);
    }
    
    public void activePlayer(bool v)
    {
        menuPlayerMovement.setActive(v);
    }

    public void exitToMenu()
    {
        if (!exiting)
        {
            exiting = true;
            sceneToChange = "Menu";
            activePlayer(false);
            menuPlayerMovement.launch();
            camera.setLooking(false);
            camera.setActive(false);
            // inOut.setExit();
            //inOut.sceneOut();
            fadeOutScript.exit();
        }
    }

    public void exitToCredits()
    {
        if (!exiting)
        {
            exiting = true;
            sceneToChange = "Credits";
            activePlayer(false);
            menuPlayerMovement.launch();
            camera.setLooking(false);
            camera.setActive(false);
            fadeOutScript.exit();            
        }
    }
}
