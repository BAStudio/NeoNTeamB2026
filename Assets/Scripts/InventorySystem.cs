using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    public System.Action OnInventoryChanged;
    public int CurrentSlot => currentSlot;
    public Transform handTransform;

    private GameObject[] slots = new GameObject[5];
    public int currentSlot = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Slot wisselen 
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeSlot(4);
    }
    public int GetCurrentSlotIndex()
    {
        return currentSlot;
    }

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;

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

                
                OnInventoryChanged?.Invoke();

                Debug.Log("Item added to slot " + (i + 1));
                PrintInventory();
                return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }

    public GameObject GetItemInSlot(int index)
    {
        if (index >= 0 && index < slots.Length)
            return slots[index];
        return null;
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

        OnInventoryChanged?.Invoke();

        Debug.Log("Switched to slot " + (currentSlot + 1));
        PrintInventory();
    }

    public void DropCurrentItem()
    {
        if (slots[currentSlot] == null) return;

        GameObject item = slots[currentSlot];
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

        slots[currentSlot] = null; 

        
        OnInventoryChanged?.Invoke();

        Debug.Log("Dropped: " + item.name);
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