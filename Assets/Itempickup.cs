using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public string itemName = "Item";
    public float pickupRange = 3f;
    public Transform handTransform;

    private GameObject heldItem;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
            {
                TryPickup();
            }
            else
            {
                DropItem();
            }
        }

        
        if (heldItem != null)
        {
            heldItem.transform.position = handTransform.position;
            heldItem.transform.rotation = handTransform.rotation;
        }
    }

    void TryPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            if (hit.transform.CompareTag("Item"))
            {
                heldItem = hit.transform.gameObject;

                
                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }

                
                heldItem.transform.SetParent(handTransform);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;

                Debug.Log("Picked up " + heldItem.name);
            }
        }
    }

    void DropItem()
    {
        if (heldItem == null) return;

        
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;

            
            rb.AddForce(playerCamera.transform.forward * 5f, ForceMode.Impulse);
        }

        
        heldItem.transform.SetParent(null);
        Debug.Log("Dropped " + heldItem.name);
        heldItem = null;
    }
}
