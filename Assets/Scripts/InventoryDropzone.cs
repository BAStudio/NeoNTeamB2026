using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDropzone : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotIndex; // Slot1 = 0, Slot2 = 1, etc.
    [SerializeField] private GameObject itemUIPrefab; // Sleep hier je ItemUI prefab in

    private GameObject currentItemUI; // Het zichtbare dragbare item in deze inventory slot
    private ItemData currentItemData; // Wat zit er in deze inventory slot?

    public int SlotIndex => slotIndex;

    public bool IsEmpty()
    {
        return currentItemData == null;
    }

    public void SetItem(ItemData itemData)
    {
        currentItemData = itemData; // Slaat op welk item in deze slot zit

        if (currentItemUI != null)
            Destroy(currentItemUI); // Verwijdert oude UI als die er was

        currentItemUI = Instantiate(itemUIPrefab, transform);
        currentItemUI.transform.SetAsLastSibling();

        Item itemScript = currentItemUI.GetComponent<Item>();
        if (itemScript != null)
        {
            itemScript.Setup(itemData); // Geeft Metal/GunPowder/Ammo data mee
            itemScript.SetOriginInventorySlot(this); // Onthoudt dat dit item uit deze inventory slot komt
        }

        RectTransform rt = currentItemUI.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.localScale = Vector3.one;
            rt.anchoredPosition = Vector2.zero;
        }
    }

    public void ClearSlot()
    {
        currentItemData = null; // Maakt inventory slot leeg

        if (currentItemUI != null)
        {
            Destroy(currentItemUI);
            currentItemUI = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        Item draggedItem = eventData.pointerDrag.GetComponent<Item>();
        if (draggedItem == null || draggedItem.itemData == null) return;

        if (!IsEmpty())
        {
            Debug.Log("Inventory slot is al bezet.");
            return;
        }

        ItemData itemData = draggedItem.itemData;

        if (itemData.prefab == null)
        {
            Debug.LogError("Geen prefab gekoppeld aan item: " + itemData.itemName);
            return;
        }

        GameObject newItem = Instantiate(itemData.prefab); // Maakt echt inventory-item aan

        ItemPickup pickup = newItem.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            pickup.itemData = itemData;
            pickup.itemID = itemData.itemName;
        }

        bool added = InventorySystem.Instance.AddItemToInventorySlot(slotIndex, newItem);

        if (added)
        {
            SetItem(itemData); // Laat item zichtbaar én dragbaar in deze inventory slot worden

            Slot parentSlot = draggedItem.OriginSlot;

            if (parentSlot != null && parentSlot.IsResultSlot)
            {
                CraftingManager cm = FindFirstObjectByType<CraftingManager>();
                if (cm != null)
                    cm.ConsumeIngredients(); // Verbruikt crafting ingredients
            }

            draggedItem.MarkDropped();
            Destroy(draggedItem.gameObject);

            Debug.Log(itemData.itemName + " toegevoegd aan inventory slot " + (slotIndex + 1));
        }
        else
        {
            Destroy(newItem);
        }
    }
}