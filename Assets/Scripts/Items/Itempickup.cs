using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public Transform handAttachPoint;

    private Camera cam;
    private GameObject heldItem;

    void Start()
    {
        cam = Camera.main;

        if (handAttachPoint == null)
        {
            GameObject t = new GameObject("HandAttachPoint");
            t.transform.SetParent(cam.transform);
            t.transform.localPosition = new Vector3(0.5f, -0.5f, 1.5f);
            handAttachPoint = t.transform;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryPickup();

        if (Input.GetKeyDown(KeyCode.Q))
            DropItem();

        if (heldItem != null)
        {
            heldItem.transform.position = handAttachPoint.position;
            heldItem.transform.rotation = handAttachPoint.rotation;
        }
    }

    void TryPickup()
    {
        if (heldItem != null)
            return;

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickupRange))
        {
            if (hit.transform.CompareTag("Item"))
            {
                heldItem = hit.transform.gameObject;

                // Zet het item in de hand
                heldItem.transform.position = handAttachPoint.position;

                // Fysica uitzetten zodat hij niet valt
                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }
            }
        }
    }

    void DropItem()
    {
        if (heldItem == null)
            return;

        // Loslaten
        heldItem.transform.SetParent(null);

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        heldItem = null;
    }
}