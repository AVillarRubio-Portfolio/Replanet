using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Interactable
{
    [SerializeField] private GameObject bonePrefab;

    private bool constructed = false;
    private GameObject constructModel;
    private GameObject aliveModel;

    void Start()
    {
        constructModel = transform.GetChild(0).gameObject;
        aliveModel = transform.GetChild(1).gameObject;
    }

    public override void Interact()
    {
        if (!constructed && PlayerController.Instance.HasItem() && PlayerController.Instance.GetCurrentItem().GetItemType() == ItemType.Cow)
        {
            ConstructCow();
        }
    }

    void ConstructCow()
    {
        constructed = true;
        PlayerController.Instance.ClearHand();
        constructModel.SetActive(false);
        aliveModel.SetActive(true);
        aliveModel.GetComponent<VoxelAnimator>().PlayAnimation("Idle");

        GetComponent<FMODUnity.StudioEventEmitter>().Play();

        LevelManager.Instance.addToActionList(new LevelAction(ActionType.CowDied));
        LevelManager.Instance.addToActionList(new LevelAction(ActionType.SpawnCow));
        LevelManager.Instance.PerformAction(ActionType.SpawnCow);

        if (!LevelManager.Instance.AllTreesArePlanted())
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
		LevelManager.Instance.PerformAction(ActionType.CowDied);
		yield return new WaitForSeconds(2f);

        aliveModel.GetComponent<VoxelAnimator>().PlayAnimation("Die");

        yield return new WaitForSeconds(2f);

        Transform bone =  Instantiate(bonePrefab).transform;
        bone.position = transform.position - transform.forward;
        bone.parent = null;

        
       
        aliveModel.SetActive(false);
        constructModel.SetActive(true);
        constructed = false;

        yield return null;
    }

    public void EnableObject()
    {
        transform.gameObject.SetActive(true);
    }
}
