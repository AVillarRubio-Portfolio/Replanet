using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct Recipe
{
    public ItemType itemType;
    public GameObject outputPrefab;
    public Sprite hintPNG;
    public bool locked;
}

public class Ovni : Interactable
{
    [SerializeField] private ItemType initialHint;
    [SerializeField] private Recipe[] recipes;

    private Dictionary<ItemType, Recipe> recipesDict;
    private GameObject hint;
    private SpriteRenderer hintRenderer;


    void Awake()
    {
        recipesDict = new Dictionary<ItemType, Recipe>();

        foreach (var r in recipes)
        {
            recipesDict.Add(r.itemType, r);
        }

        hint = transform.GetChild(0).gameObject;
        hintRenderer = hint.transform.GetChild(0).GetComponent<SpriteRenderer>();
        HideHint();

        if(initialHint != ItemType.None)
            ShowHint(initialHint);
    }

    public override void Interact()
    {
        if (PlayerController.Instance.HasItem())
        {
            ItemType itemType = PlayerController.Instance.GetCurrentItem().GetItemType();

            if (recipesDict.ContainsKey(itemType) && !recipesDict[itemType].locked)
            {
                PlayerController.Instance.ClearHand();

                Transform itemToHold = Instantiate(recipesDict[itemType].outputPrefab).transform;
                PlayerController.Instance.HoldItem(itemToHold);

                if(itemType == ItemType.Wood)
                    LevelManager.Instance.PerformAction(ActionType.CraftTree);
                else if (itemType == ItemType.Bone)
                    LevelManager.Instance.PerformAction(ActionType.CraftCow);

                GetComponent<FMODUnity.StudioEventEmitter>().Play();


                HideHint();
            }
        }
    }

    public void ShowHint(string itemType)
    {
        ItemType.TryParse(itemType, out ItemType t);
        ShowHint(t);
    }

    public void ShowHint(ItemType itemType)
    {
        if (recipesDict.ContainsKey(itemType))
        {
            hint.SetActive(true);
            hintRenderer.sprite = recipesDict[itemType].hintPNG;
        }
    }

    public void HideHint()
    {
        hint.SetActive(false);
    }

    public void LockRecipe(ItemType itemType)
    {
        if (recipesDict.ContainsKey(itemType))
        {
            Recipe recipe = recipesDict[itemType];
            recipe.locked = true;
            recipesDict[itemType] = recipe;
        }
    }

    public void UnlockRecipe(ItemType itemType)
    {
        if (recipesDict.ContainsKey(itemType))
        {
            Recipe recipe = recipesDict[itemType];
            recipe.locked = false;
            recipesDict[itemType] = recipe;
        }
    }

    public void UnlockRecipe(string itemType)
    {
        ItemType.TryParse(itemType, out ItemType t);
        UnlockRecipe(t);
    }

    public void LockRecipe(string itemType)
    {
        ItemType.TryParse(itemType, out ItemType t);
        LockRecipe(t);
    }
}
