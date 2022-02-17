using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Interactable
{
    public override void Interact()
    {
        if (PlayerController.Instance.HasItem() && PlayerController.Instance.GetCurrentItem().GetItemType() == ItemType.Bucket)
        {
            Bucket bucket = (Bucket)PlayerController.Instance.GetCurrentItem();
            bucket.TryFillBucket();
        }
    }
}
