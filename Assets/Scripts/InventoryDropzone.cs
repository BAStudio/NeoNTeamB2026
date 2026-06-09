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

        
        // HOTBAR  INVENTORY
        
        if (draggedItem.OriginHotbarSlot != null)
        {
            int hotbarIndex = draggedItem.OriginHotbarSlot.HotbarIndex;
            // Dit is de hotbar-slot waar het item vandaan komt

            bool moved = InventorySystem.Instance.MoveHotbarItemToInventorySlot(hotbarIndex, slotIndex);
            // Verplaatst het bestaande item van hotbar naar inventory
            // Dus GEEN nieuwe prefab maken

            if (moved)
            {
                SetItem(itemData);
                // Zet het item zichtbaar in deze inventory-slot

                draggedItem.OriginHotbarSlot.ClearSlot();
                // Maakt de hotbar-slot visueel leeg

                draggedItem.MarkDropped();
                // Zegt tegen Item.cs dat de drop gelukt is

                Destroy(draggedItem.gameObject);
                // Verwijdert alleen het gesleepte UI-plaatje, niet het echte item

                Debug.Log(itemData.itemName + " verplaatst van hotbar naar inventory slot " + (slotIndex + 1));
            }

            return;
            // Heel belangrijk: stop hier, anders maakt hij hieronder een kopie aan
        }

         
        // CRAFTING RESULT INVENTORY
        
        if (itemData.prefab == null)
        {
            Debug.LogError("Geen prefab gekoppeld aan item: " + itemData.itemName);
            return;
        }

        GameObject newItem = Instantiate(itemData.prefab);
        // Alleen bij crafting/result mag hij een nieuw item maken

        ItemPickup pickup = newItem.GetComponent<ItemPickup>();

        if (pickup != null)
        {
            pickup.itemData = itemData;
            pickup.itemID = itemData.itemName;
        }

        bool added = InventorySystem.Instance.AddItemToInventorySlot(slotIndex, newItem);

        if (added)
        {
            SetItem(itemData);

            Slot parentSlot = draggedItem.OriginSlot;

            if (parentSlot != null && parentSlot.IsResultSlot)
            {
                CraftingManager cm = FindFirstObjectByType<CraftingManager>();

                if (cm != null)
                {
                    cm.ConsumeIngredients();
                }
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