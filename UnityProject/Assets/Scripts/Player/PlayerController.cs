using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static public PlayerController Instance;

    [SerializeField] private float aimSensitivity = 10f;
    [Range(0f, 1f)] [SerializeField] private float buildUpRot;
    [SerializeField] private float minInput;
    [SerializeField] private Vector3 pickUpZoneSize = new Vector3(3,1,3);

    // Aim
    private Vector3 forward;
    private Vector3 right;

    private PlayerInputHandler playerInput;

    // Items
    private Transform playerHand;
    private Item currentItem;

    #region Gets
    public float GetMinInput() { return minInput; }
    public float GetBuildUpRot() { return buildUpRot; }

    public float GetItemWeight()
    {
        if (currentItem == null) return 0;
        else return currentItem.GetWeight();
    }

    public Item GetCurrentItem() { return currentItem; }
    public bool HasItem() { return currentItem != null; }

    #endregion

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        playerInput = GetComponent<PlayerInputHandler>();
        playerHand = transform.Find("Hand");
    }

    void OnDrawGizmos()
    {
#if EDITOR
        if (EditorApplication.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3Int.RoundToInt(transform.position + transform.forward), pickUpZoneSize);

            Gizmos.color = Color.blue;
            Vector2 pos = GetCenterPos2D();
            Vector3 p = new Vector3(pos.x, 1.1f, pos.y);
            Gizmos.DrawWireCube(p, new Vector3(0.9f, 0.9f, 0.9f));
        }
#endif
    }


    Vector2Int GetCenterPos2D()
    {
        return VectorOperations.Vector3ToVector2Int(transform.position + transform.forward * 0.4f);
    }

    Vector2Int GetAimPos2D()
    {
        return VectorOperations.Vector3ToVector2Int(transform.position + transform.forward);
    }

    private void PickUpItem()
    {
        if (currentItem == null)
        {
            Interact(false);        
        }
    }

    private void Interact(bool reachFurther)
    {
        GameObject interactable = GetInteractable(reachFurther);

        if (interactable != null)
        {
            interactable.GetComponent<Interactable>().Interact();
            
        }
    }

    GameObject GetInteractable(bool reachFurther)
    {
        GameObject interactable = null;

        Collider[] objects = Physics.OverlapBox(transform.position + (transform.forward * (reachFurther? 0.6f : 0.2f)), pickUpZoneSize);

        float minDistance = float.MaxValue;
        foreach (Collider o in objects)
        {
            if (o.CompareTag("Interactable"))
            {
                float objDistance = Vector3.Distance(transform.position, o.transform.position);
                if (objDistance < minDistance)
                {
                    minDistance = objDistance;
                    interactable = o.gameObject;
                }
            }
        }

        return interactable;
    }

    bool CanReleaseItem()
    {
        Vector2 pos = GetCenterPos2D();

        Collider[] objects = Physics.OverlapBox(new Vector3(pos.x, 1.1f, pos.y), new Vector3(0.45f, 0.45f, 0.45f));

        foreach (Collider o in objects)
        {
            if (o.tag != "Player")
                return false;
        }

        return true;
    }

    private void ReleaseItem()
    {
        if (currentItem != null)
        {
            if (CanReleaseItem())
            {
                Vector2 pos = GetCenterPos2D();

                currentItem.transform.position = new Vector3(pos.x, 0.5f, pos.y);
                currentItem.transform.parent = null;
                currentItem.transform.localRotation = Quaternion.identity;
                currentItem.GetComponent<BoxCollider>().enabled = true;

                currentItem = null;
            }
        }
    }

    public void PickUpRelease()
    {
        if(currentItem == null)
            PickUpItem();
        else
        {
            if(TryInteract())
                return;

            ReleaseItem();
        }
    }

    // Returns true if the player has interacted
    private bool TryInteract()
    {
        if (!CanReleaseItem())
        {
            Interact(true);
        }

        return false;
    }

    // Set item in player hand
    public void HoldItem(Transform item)
    {

        if (currentItem == null)
        {
            GetComponent<FMODUnity.StudioEventEmitter>().Event = "event:/Pick";
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
        }

        currentItem = item.GetComponent<Item>();
        item.GetComponent<BoxCollider>().enabled = false;
        
        item.parent = playerHand;
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
        
    }

    public void ClearHand()
    {
        Destroy(currentItem.gameObject);
        currentItem = null;
    }
}
