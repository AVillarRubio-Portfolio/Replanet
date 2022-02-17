using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveTree : Interactable
{
    public override void Interact()
    {
        if (PlayerController.Instance.HasItem() && PlayerController.Instance.GetCurrentItem().GetItemType() == ItemType.Bucket)
        {
            Bucket bucket = (Bucket)PlayerController.Instance.GetCurrentItem();
            if (bucket.IsBucketFull())
            {
                bucket.TryEmptyBucket();
                GetComponent<ChangeMyZoneScript>().poblateZone();
                LevelManager.Instance.PerformAction(ActionType.WaterTree);
            }
        }
    }
}
