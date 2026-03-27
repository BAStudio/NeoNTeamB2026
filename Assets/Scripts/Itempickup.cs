using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
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

        if (Physics.Raycast(playerCamera.transform.position,
                            playerCamera.transform.forward,
                            out hit,
                            pickupRange))
        {
            if (hit.transform.CompareTag("Item"))
            {
                GameObject item = hit.transform.gameObject;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }

                bool added = InventorySystem.Instance.AddItem(item);

                if (added)
                {
                    Debug.Log("Picked up: " + item.name);
                }
                else
                {
                    // inventory vol → physics terugzetten
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