using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Interactable
{
    [SerializeField] private GameObject[] neighbourFires;

    public override void Interact()
    {
        if (PlayerController.Instance.HasItem() && PlayerController.Instance.GetCurrentItem().GetItemType() == ItemType.Bucket)
        {
            Bucket bucket = (Bucket)PlayerController.Instance.GetCurrentItem();
            if (bucket.IsBucketFull())
            {
                bucket.TryEmptyBucket();
                //LevelManager.Instance.PerformAction(ActionType.WaterTree);

                foreach (var f in neighbourFires)
                {
                    Destroy(f);
                }

                Destroy(this.gameObject);
            }
        }
    }
}
