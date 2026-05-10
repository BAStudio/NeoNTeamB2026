using UnityEngine; 

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    [Header("Item Identification")]
    public string itemID;

    public float pickupRange = 3f;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;

        if (itemData != null)
            itemID = itemData.itemName;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            InventorySystem.Instance.DropCurrentItem();
        }
    }

    void TryPickup()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            if (hit.transform.CompareTag("Item"))
            {
                GameObject item = hit.transform.gameObject;

                ItemPickup pickup = item.GetComponent<ItemPickup>();

                if (pickup == null || pickup.itemData == null)
                {
                    Debug.LogError("Dit item mist ItemPickup of ItemData.");
                    return;
                }

                Rigidbody rb = item.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }

                bool added = InventorySystem.Instance.AddItemToInventory(item);
                // Hierdoor komt Metal echt in je InventoryUI, niet los als plaatje

                if (added)
                {
                    Debug.Log("Picked up: " + item.name);
                }
                else
                {
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.detectCollisions = true;
                    }
                }
            }
        }
    }
}