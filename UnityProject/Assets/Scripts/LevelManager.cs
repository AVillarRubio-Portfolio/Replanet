using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Cinemachine;

public enum ActionType
{
    PlantTree,
    BuildBridge,
    WaterTree,
    SpawnCow,
    CraftTree,
    CraftCow,
    CowDied,
    TreeDied,
    PickUpItem
}

[System.Serializable]
public struct LevelAction
{
    public LevelAction(ActionType action)
    {
        actionType = action;
        functionsWhenFinished = new UnityEvent();

    }

	public LevelAction(ActionType action, UnityEvent ev)
	{
		actionType = action;
		functionsWhenFinished = ev;
	}

	public ActionType actionType;
    public UnityEvent functionsWhenFinished;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private float timeToChangeScene = 4f;
    [SerializeField] private GameCircleInOut inOut;

    [Header("Functions called at level start")]
    [SerializeField] private UnityEvent actionsBeforeStart = new UnityEvent();

    [Header("Action to complete the level")]
    [SerializeField] private LevelAction[] levelActions;

    public CinemachineVirtualCamera cam;

    private List<LevelAction> actionsList;

	public void addToActionList(LevelAction l)
	{
		actionsList.Add(l);
	}
    
    public int treesToPlant = 0;
    public int treesPlanted = 0;

    private int bridgesToBuild = 0;
    private int bridgesBuilt = 0;

    private int cowsToSpawn = 0;
    private int cowsSpawned = 0;

	private int treesToCraft = 0;
	private int treesCrafted = 0;

    private int treesToWater = 0;
    private int treesWatered = 0;

    public bool AllTreesArePlanted() { return treesPlanted >= treesToPlant; }

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        actionsList = new List<LevelAction>();
        foreach (var action in levelActions)
        {
            actionsList.Add(action);
            if (action.actionType == ActionType.PlantTree)
                treesToPlant++;
			else if (action.actionType == ActionType.CraftTree)
				treesToCraft++;
            else if (action.actionType == ActionType.BuildBridge)
                bridgesToBuild++;
            else if (action.actionType == ActionType.SpawnCow)
                cowsToSpawn++;
            else if (action.actionType == ActionType.WaterTree)
                treesToWater++;
        }

        actionsBeforeStart.Invoke();
    }

    public void PerformAction(ActionType actionType)
    {
        if (actionsList.Count > 0)
        {
            bool found = false;
            for (int i = 0; i < actionsList.Count && !found; i++)
            {
                if (actionType == actionsList[i].actionType)
                {
                    found = true;
                    LevelAction action = actionsList[i];
                    actionsList.RemoveAt(i);

                    switch (actionType)
                    {
                        case ActionType.PlantTree:
                            treesPlanted++;
                            break;
                        case ActionType.BuildBridge:
                            bridgesBuilt++;
                            break;
                        case ActionType.WaterTree:
                            treesWatered++;
                            break;
                        case ActionType.SpawnCow:
                            cowsSpawned++;
                            break;
                        case ActionType.CowDied:
                            cowsSpawned--;
                            break;
                        case ActionType.TreeDied:
                            treesPlanted--;
                            break;
                        case ActionType.CraftTree:
                            treesCrafted++;
                            break;
                    }

                    action.functionsWhenFinished.Invoke();
                }
            }
        }
		CheckIfLevelCompleted();
	}

    public void CheckIfLevelCompleted()
    {
        if (treesPlanted >= treesToPlant && 
            treesWatered >= treesToWater &&
            bridgesBuilt >= bridgesToBuild &&
            cowsSpawned >= cowsToSpawn &&
			treesCrafted >= treesToCraft)
        {
            //Debug.Log("LevelFinished!");
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
            Invoke(nameof(loadScene), timeToChangeScene);
            unlockNextLevel();
        }
    }

    private void unlockNextLevel()
    {
        Globals.lastLevelDone = true;
    }

    public void exitLevel()
    {
        // No te has pasado el nivel
        if(!(treesPlanted >= treesToPlant &&
            treesWatered >= treesToWater &&
            bridgesBuilt >= bridgesToBuild &&
            cowsSpawned >= cowsToSpawn &&
            treesCrafted >= treesToCraft))
        {
            loadScene();
        }
    }

    public void loadScene()
    {
        cam.enabled = false;
        inOut.sceneOut();
    }

    public void changeScene()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}
