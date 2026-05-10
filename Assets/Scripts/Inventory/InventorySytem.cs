using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    // Hiermee kunnen andere scripts jouw inventory vinden via InventorySystem.Instance

    public System.Action OnInventoryChanged;
    // Wordt aangeroepen als inventory of hotbar verandert

    [Header("References")]
    public Transform handTransform;
    // Hier komt een item terecht als je het in je hand hebt

    [SerializeField] private InventoryDropzone[] inventorySlotsUI;
    // Hier sleep je Slot1 t/m Slot12 in via de Inspector
    // Element 0 = Slot1
    // Element 1 = Slot2
    // Element 2 = Slot3
    // enzovoort

    [Header("Settings")]
    public int currentSlot = 0;
    // Actieve hotbar slot
    // 0 = Hotbar slot 1
    // 1 = Hotbar slot 2
    // enzovoort

    public int CurrentSlot => currentSlot;
    // Andere scripts kunnen hiermee lezen welke hotbar slot actief is

    private const int hotbarSlotCount = 5;
    // Hotbar heeft 5 slots

    private const int inventorySlotCount = 12;
    // Inventory heeft 12 slots

    private GameObject[] slots = new GameObject[17];
    // Echte opslag:
    // index 0 t/m 4 = hotbar
    // index 5 t/m 16 = inventory

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Deze wordt de enige actieve InventorySystem
        }
        else
        {
            Destroy(gameObject);
            // Voorkomt dubbele InventorySystems
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeSlot(4);

        if (Input.GetKeyDown(KeyCode.Q)) DropCurrentItem();
    }

    public bool AddItemToInventory(GameObject item)
    {
        if (item == null)
        {
            Debug.LogError("Item is null en kan niet worden toegevoegd.");
            return false;
        }

        ItemPickup pickup = item.GetComponent<ItemPickup>();
        // Pakt de iteminformatie van het opgepakte object

        if (pickup == null)
        {
            Debug.LogError("Item mist ItemPickup script: " + item.name);
            return false;
        }

        if (pickup.itemData == null)
        {
            Debug.LogError("Item mist ItemData: " + item.name);
            return false;
        }

        for (int i = 0; i < inventorySlotCount; i++)
        {
            int realIndex = i + hotbarSlotCount;
            // Inventory Slot1 = echte index 5
            // Inventory Slot2 = echte index 6
            // Inventory Slot3 = echte index 7

            if (slots[realIndex] == null)
            {
                bool added = AddItemToInventorySlot(i, item);
                // Zet item in de echte inventory-data

                if (added)
                {
                    Debug.Log("Item zit in inventory data. Nu UI vullen op inventory slot: " + i);

                    if (inventorySlotsUI == null)
                    {
                        Debug.LogError("inventorySlotsUI is niet ingevuld op InventorySystem.");
                        return added;
                    }

                    if (i >= inventorySlotsUI.Length)
                    {
                        Debug.LogError("inventorySlotsUI is te kort. Index bestaat niet: " + i);
                        return added;
                    }

                    if (inventorySlotsUI[i] == null)
                    {
                        Debug.LogError("Inventory Slots UI Element " + i + " is leeg op InventorySystem.");
                        return added;
                    }

                    inventorySlotsUI[i].SetItem(pickup.itemData);
                    // Dit maakt het item zichtbaar in je inventory UI

                    Debug.Log("UI gevuld op inventory slot: " + i + " met item: " + pickup.itemData.itemName);
                }

                return added;
            }
        }

        Debug.Log("Inventory is vol.");
        return false;
    }

    public bool AddItemToInventorySlot(int inventorySlotIndex, GameObject item)
    {
        if (inventorySlotIndex < 0 || inventorySlotIndex >= inventorySlotCount)
        {
            Debug.LogWarning("Inventory slot bestaat niet: " + inventorySlotIndex);
            return false;
        }

        int realSlotIndex = inventorySlotIndex + hotbarSlotCount;
        // Inventory Slot1 wordt echte index 5

        return AddItemToSlot(realSlotIndex, item);
    }

    public bool AddItemToSlot(int slotIndex, GameObject item)
    {
        if (item == null)
        {
            Debug.LogWarning("Item is null.");
            return false;
        }

        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogWarning("Slot bestaat niet: " + slotIndex);
            return false;
        }

        if (slots[slotIndex] != null)
        {
            Debug.Log("Slot " + slotIndex + " is al bezet.");
            return false;
        }

        slots[slotIndex] = item;
        // Item wordt opgeslagen in de echte data-array

        if (slotIndex == currentSlot)
        {
            item.SetActive(true);

            if (handTransform != null)
            {
                item.transform.SetParent(handTransform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            item.SetActive(false);
            // Inventory-items blijven verborgen in de wereld
        }

        OnInventoryChanged?.Invoke();

        Debug.Log("Item toegevoegd aan echte slot index: " + slotIndex);

        return true;
    }

    public GameObject GetItemInSlot(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            return slots[index];
        }

        return null;
    }

    public GameObject GetItemInInventorySlot(int inventorySlotIndex)
    {
        int realIndex = inventorySlotIndex + hotbarSlotCount;

        if (realIndex >= 0 && realIndex < slots.Length)
        {
            return slots[realIndex];
        }

        return null;
    }

    public int GetCurrentSlotIndex()
    {
        return currentSlot;
    }

    public void ChangeSlot(int newSlot)
    {
        if (newSlot < 0 || newSlot >= hotbarSlotCount)
        {
            Debug.LogWarning("Hotbar slot bestaat niet: " + newSlot);
            return;
        }

        if (newSlot == currentSlot) return;

        if (slots[currentSlot] != null)
        {
            slots[currentSlot].SetActive(false);
        }

        currentSlot = newSlot;

        if (slots[currentSlot] != null)
        {
            GameObject item = slots[currentSlot];

            item.SetActive(true);

            if (handTransform != null)
            {
                item.transform.SetParent(handTransform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
            }
        }

        OnInventoryChanged?.Invoke();

        Debug.Log("Gewisseld naar hotbar slot " + (currentSlot + 1));
    }

    public void DropCurrentItem()
    {
        if (currentSlot < 0 || currentSlot >= hotbarSlotCount)
        {
            Debug.LogWarning("Current slot is ongeldig: " + currentSlot);
            return;
        }

        if (slots[currentSlot] == null)
        {
            Debug.Log("Geen item om te droppen.");
            return;
        }

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

            if (Camera.main != null)
            {
                rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);
            }
        }

        slots[currentSlot] = null;

        OnInventoryChanged?.Invoke();

        Debug.Log("Gedropt: " + item.name);
    }

    public void RemoveInventorySlot(int inventorySlotIndex)
    {
        if (inventorySlotIndex < 0 || inventorySlotIndex >= inventorySlotCount)
        {
            Debug.LogWarning("Inventory slot bestaat niet: " + inventorySlotIndex);
            return;
        }

        int realSlotIndex = inventorySlotIndex + hotbarSlotCount;

        if (slots[realSlotIndex] != null)
        {
            Destroy(slots[realSlotIndex]);
        }

        slots[realSlotIndex] = null;

        if (inventorySlotsUI != null &&
            inventorySlotIndex < inventorySlotsUI.Length &&
            inventorySlotsUI[inventorySlotIndex] != null)
        {
            inventorySlotsUI[inventorySlotIndex].ClearSlot();
        }

        OnInventoryChanged?.Invoke();
    }

    public bool MoveInventoryItemToHotbar(int inventorySlotIndex)
    {
        if (inventorySlotIndex < 0 || inventorySlotIndex >= inventorySlotCount)
        {
            Debug.LogWarning("Inventory slot bestaat niet: " + inventorySlotIndex);
            return false;
        }

        int realInventoryIndex = inventorySlotIndex + hotbarSlotCount;

        GameObject item = slots[realInventoryIndex];

        if (item == null)
        {
            Debug.Log("Geen item in inventory slot " + (inventorySlotIndex + 1));
            return false;
        }

        for (int i = 0; i < hotbarSlotCount; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                slots[realInventoryIndex] = null;

                if (inventorySlotsUI != null &&
                    inventorySlotIndex < inventorySlotsUI.Length &&
                    inventorySlotsUI[inventorySlotIndex] != null)
                {
                    inventorySlotsUI[inventorySlotIndex].ClearSlot();
                }

                if (i == currentSlot)
                {
                    item.SetActive(true);

                    if (handTransform != null)
                    {
                        item.transform.SetParent(handTransform);
                        item.transform.localPosition = Vector3.zero;
                        item.transform.localRotation = Quaternion.identity;
                    }
                }
                else
                {
                    item.SetActive(false);
                }

                OnInventoryChanged?.Invoke();

                Debug.Log("Item verplaatst naar hotbar slot " + (i + 1));

                return true;
            }
        }

        Debug.Log("Hotbar is vol.");
        return false;
    }

    public bool HasItem(string idToFind)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                ItemPickup itemPickup = slots[i].GetComponent<ItemPickup>();

                if (itemPickup != null && itemPickup.itemID == idToFind)
                {
                    return true;
                }

                if (itemPickup != null &&
                    itemPickup.itemData != null &&
                    itemPickup.itemData.itemName == idToFind)
                {
                    return true;
                }
            }
        }

        return false;
    }
}