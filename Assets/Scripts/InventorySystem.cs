using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public Transform handTransform;

    private GameObject[] slots = new GameObject[3];
    private int currentSlot = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // Slot wisselen met 1,2,3
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSlot(2);
    }

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;

                // Als dit de actieve slot is → direct in hand tonen
                if (i == currentSlot)
                {
                    item.SetActive(true);
                    item.transform.SetParent(handTransform);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    item.SetActive(false);
                }

                Debug.Log("Item added to slot " + (i + 1));
                PrintInventory();
                return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }

    public void ChangeSlot(int newSlot)
    {
        if (newSlot == currentSlot) return;

        // Huidige item verbergen
        if (slots[currentSlot] != null)
            slots[currentSlot].SetActive(false);

        currentSlot = newSlot;

        // Nieuwe item tonen
        if (slots[currentSlot] != null)
        {
            GameObject item = slots[currentSlot];
            item.SetActive(true);
            item.transform.SetParent(handTransform);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }

        Debug.Log("Switched to slot " + (currentSlot + 1));
        PrintInventory();
    }

    public void DropCurrentItem()
    {
        if (slots[currentSlot] == null) return;

        GameObject item = slots[currentSlot];

        // Eerst losmaken
        item.transform.SetParent(null);

        item.SetActive(true);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);
        }

        Debug.Log("Dropped: " + item.name);

        slots[currentSlot] = null;

        PrintInventory();
    }

    void PrintInventory()
    {
        Debug.Log("=== INVENTORY ===");
        for (int i = 0; i < slots.Length; i++)
        {
            string itemName = slots[i] != null ? slots[i].name : "Empty";
            Debug.Log("Slot " + (i + 1) + ": " + itemName);
        }
    }
}