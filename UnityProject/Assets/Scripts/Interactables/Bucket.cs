using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : Item
{
    [SerializeField] private Mesh fullBucket;

    private Mesh emptyBucket;
    private bool bucketIsFull = false;
    private MeshFilter meshFilter;

    public bool IsBucketFull() { return bucketIsFull; }

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        emptyBucket = meshFilter.mesh;
    }

    public void TryFillBucket()
    {
        if (!bucketIsFull)
        {
            meshFilter.mesh = fullBucket;
            bucketIsFull = true;
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
        }
    }

    public void TryEmptyBucket()
    {
        if (bucketIsFull)
        {
            meshFilter.mesh = emptyBucket;
            bucketIsFull = false;
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
        }
    }

    public override void Interact()
    {
        if (!PlayerController.Instance.HasItem())
        {
            PlayerController.Instance.HoldItem(transform);
        }
    }
}
