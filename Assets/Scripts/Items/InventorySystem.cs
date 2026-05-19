using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    public System.Action OnInventoryChanged;
    public int CurrentSlot => currentSlot;
    public Transform handTransform;

    private GameObject[] slots = new GameObject[12];
    public int currentSlot = 0;

    private HandItemHandler handItemHandler;
    private ItemDropHandler itemDropHandler;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        handItemHandler = GetComponent<HandItemHandler>();
        itemDropHandler = GetComponent<ItemDropHandler>();

        if (handItemHandler != null)
            handItemHandler.handTransform = handTransform;
    }

    public int GetCurrentSlotIndex()
    {
        return currentSlot;
    }

    public bool AddItem(GameObject Item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = Item;

                handItemHandler.HideItem(Item);

                OnInventoryChanged?.Invoke();

                Debug.Log("Item added to slot " + (i + 1));
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
        if (newSlot < 0 || newSlot >= slots.Length) return;
        if (newSlot == currentSlot) return;

        if (slots[currentSlot] != null)
            handItemHandler.HideItem(slots[currentSlot]);

        currentSlot = newSlot;

        if (slots[currentSlot] != null)
            handItemHandler.ShowItem(slots[currentSlot]);

        OnInventoryChanged?.Invoke();

        Debug.Log("Switched to slot " + (currentSlot + 1));
    }

    public void DropCurrentItem()
    {
        if (slots[currentSlot] == null) return;

        GameObject item = slots[currentSlot];
        handItemHandler.HideItem(item);
        itemDropHandler.Drop(item);
        slots[currentSlot] = null;

        OnInventoryChanged?.Invoke();

        InventoryQuickaccess quickAccess = FindObjectOfType<InventoryQuickaccess>();
        if (quickAccess != null)
        {
            quickAccess.ClearDroppedItems();
        }

        Debug.Log("Dropped: " + item.name);
    }

    public bool HasItem(string idToFind)
    {
        foreach (GameObject itemObj in slots)
        {
            if (itemObj != null)
            {
                ItemPickup details = itemObj.GetComponent<ItemPickup>();
                if (details != null && details.itemID == idToFind)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
