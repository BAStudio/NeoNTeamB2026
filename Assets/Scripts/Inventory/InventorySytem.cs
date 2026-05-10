using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public System.Action OnInventoryChanged;
    public Transform handTransform;
    public int CurrentSlot => currentSlot;//// Hiermee kunnen andere scripts lezen welke hotbar slot actief is

    [SerializeField] private InventoryDropzone[] inventorySlotsUI;
    // Hier sleep je Slot1 t/m Slot12 in, zodat de UI zichtbaar wordt bijgewerkt

    private GameObject[] slots = new GameObject[17];
    // 0 t/m 4 = hotbar
    // 5 t/m 16 = inventory

    public int currentSlot = 0;

    private const int hotbarSlotCount = 5;
    private const int inventorySlotCount = 12;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
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
        ItemPickup pickup = item.GetComponent<ItemPickup>();

        if (pickup == null || pickup.itemData == null)
        {
            Debug.LogError("Item mist ItemPickup of ItemData.");
            return false;
        }

        for (int i = 0; i < inventorySlotCount; i++)
        {
            int realIndex = i + hotbarSlotCount;

            if (slots[realIndex] == null)
            {
                bool added = AddItemToInventorySlot(i, item);

                if (added && inventorySlotsUI != null && i < inventorySlotsUI.Length)
                {
                    inventorySlotsUI[i].SetItem(pickup.itemData);
                    // Dit maakt het item zichtbaar én dragbaar in je inventory
                }

                return added;
            }
        }

        Debug.Log("Inventory is vol.");
        return false;
    }

    public bool AddItemToInventorySlot(int inventorySlotIndex, GameObject item)
    {
        int realSlotIndex = inventorySlotIndex + hotbarSlotCount;
        return AddItemToSlot(realSlotIndex, item);
    }

    public bool AddItemToSlot(int slotIndex, GameObject item)
    {
        if (item == null) return false;

        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogWarning("Slot bestaat niet: " + slotIndex);
            return false;
        }

        if (slots[slotIndex] != null)
        {
            Debug.Log("Slot is al bezet: " + slotIndex);
            return false;
        }

        slots[slotIndex] = item;

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
            item.SetActive(false); // Inventory items zijn verborgen in de wereld
        }

        OnInventoryChanged?.Invoke();

        Debug.Log("Item toegevoegd aan echte slot index: " + slotIndex);
        return true;
    }

    public void RemoveInventorySlot(int inventorySlotIndex)
    {
        int realSlotIndex = inventorySlotIndex + hotbarSlotCount;

        if (realSlotIndex < 0 || realSlotIndex >= slots.Length) return;

        slots[realSlotIndex] = null;

        if (inventorySlotsUI != null && inventorySlotIndex < inventorySlotsUI.Length)
        {
            inventorySlotsUI[inventorySlotIndex].ClearSlot();
        }

        OnInventoryChanged?.Invoke();
    }

    public void ChangeSlot(int newSlot)
    {
        if (newSlot < 0 || newSlot >= hotbarSlotCount) return;

        if (slots[currentSlot] != null)
            slots[currentSlot].SetActive(false);

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

            if (Camera.main != null)
                rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);
        }

        slots[currentSlot] = null;
        OnInventoryChanged?.Invoke();
    }
    public GameObject GetItemInInventorySlot(int inventorySlotIndex)
    {
        int realIndex = inventorySlotIndex + hotbarSlotCount;
        // Inventory Slot1 = echte index 5

        if (realIndex >= 0 && realIndex < slots.Length)
        {
            return slots[realIndex];
            // Geeft het item uit de inventory terug
        }

        return null;
        // Geeft niks terug als index fout is
    }
    public bool MoveInventoryItemToHotbar(int inventorySlotIndex)
    {
        int realInventoryIndex = inventorySlotIndex + hotbarSlotCount;
        // Zet inventory index om naar echte array index

        if (realInventoryIndex < hotbarSlotCount || realInventoryIndex >= slots.Length)
        {
            Debug.LogWarning("Inventory slot bestaat niet.");
            return false;
        }

        GameObject item = slots[realInventoryIndex];

        if (item == null)
        {
            Debug.Log("Geen item om te verplaatsen.");
            return false;
        }

        for (int i = 0; i < hotbarSlotCount; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                // Zet item in eerste lege hotbar slot

                slots[realInventoryIndex] = null;
                // Maakt inventory slot leeg

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
    public int GetCurrentSlotIndex()
    {
        return currentSlot;
        // Geeft terug welke hotbar slot actief is
        // 0 = hotbar slot 1
        // 1 = hotbar slot 2
        // enzovoort
    }

    public GameObject GetItemInSlot(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            return slots[index];
            // Geeft het item terug dat in deze slot zit
        }

        return null;
        // Als de index niet bestaat, geef niks terug
    }
    public bool HasItem(string idToFind)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // Loopt door alle slots heen: hotbar + inventory

            if (slots[i] != null)
            {
                // Checkt alleen slots waar echt een item in zit

                ItemPickup itemPickup = slots[i].GetComponent<ItemPickup>();
                // Pakt het ItemPickup script van het item

                if (itemPickup != null && itemPickup.itemID == idToFind)
                {
                    return true;
                    // Item gevonden
                }

                if (itemPickup != null && itemPickup.itemData != null && itemPickup.itemData.itemName == idToFind)
                {
                    return true;
                    // Extra check via ItemData naam
                }
            }
        }

        return false;
        // Item niet gevonden
    }
}