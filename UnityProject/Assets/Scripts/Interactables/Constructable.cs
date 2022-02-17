using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructable : Interactable
{
    [SerializeField] private ItemType itemNeeded;
    [SerializeField] public ActionType actionPerformed;

    private bool constructed = false;
    private GameObject transparentObject;
    [HideInInspector] public GameObject contructedObject;

    void Awake()
    {
        transparentObject = transform.GetChild(0).gameObject;
        contructedObject = transform.GetChild(1).gameObject;
    }

    public override void Interact()
    {
        if (!constructed && PlayerController.Instance.HasItem() && PlayerController.Instance.GetCurrentItem().GetItemType() == itemNeeded)
        {
            Construct();
        }
    }

    private void Construct()
    {
        LevelManager.Instance.PerformAction(actionPerformed);
        PlayerController.Instance.ClearHand();

        transparentObject.SetActive(false);
        contructedObject.SetActive(true);

        ChangeMyZoneScript zone = contructedObject.GetComponent<ChangeMyZoneScript>();
        if (zone != null)
            zone.poblateZone();

        if (actionPerformed == ActionType.PlantTree)
        {
            StartCoroutine(Die());
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
            GetComponent<BoxCollider>().isTrigger = false;
        }

        if(actionPerformed == ActionType.BuildBridge)
        {
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Deconstruct()
    {
        if (constructed)
        {
            constructed = false;

            contructedObject.SetActive(false);
            transparentObject.SetActive(true);

            LevelManager.Instance.PerformAction(ActionType.TreeDied);
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2f);

        Deconstruct();

        yield return null;
    }

    public void EnableObject()
    {
        transform.gameObject.SetActive(true);
    }

    public void EnableObjectWithDelay(float delay)
    {
        Invoke(nameof(EnableObject), delay);
    }

    public void DisableObject()
    {
        transform.gameObject.SetActive(false);
    }
}
