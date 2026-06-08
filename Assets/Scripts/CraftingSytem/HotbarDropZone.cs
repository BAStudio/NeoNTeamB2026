using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarDropzone : MonoBehaviour, IDropHandler
{
    [SerializeField] private int hotbarIndex;
    // Hotbar Slot 1 = 0
    // Hotbar Slot 2 = 1
    // Hotbar Slot 3 = 2
    // Hotbar Slot 4 = 3
    // Hotbar Slot 5 = 4

    [SerializeField] private GameObject itemUIPrefab;
    // Hier sleep je jouw ItemUI prefab in

    private GameObject currentItemUI;
    // Dit is het zichtbare dragbare item in deze hotbar slot

    private ItemData currentItemData;
    // Dit onthoudt welk item in deze hotbar slot zit

    public int HotbarIndex => hotbarIndex;
    // Hiermee kan Item.cs weten uit welke hotbar slot het item komt

    public void SetItem(ItemData itemData)
    {
        currentItemData = itemData;
        // Slaat de ItemData op

        if (currentItemUI != null)
        {
            Destroy(currentItemUI);
            // Verwijdert oud hotbar UI item
        }

        currentItemUI = Instantiate(itemUIPrefab, transform);
        // Maakt zichtbaar item in deze hotbar slot

        currentItemUI.transform.SetAsLastSibling();
        // Zet item bovenop de slot

        Item itemScript = currentItemUI.GetComponent<Item>();

        if (itemScript != null)
        {
            itemScript.Setup(itemData);
            // Geeft juiste ItemData mee

            itemScript.SetOriginHotbarSlot(this);
            // Zegt tegen Item.cs: dit item komt uit de hotbar
        }

        RectTransform rt = currentItemUI.GetComponent<RectTransform>();

        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            // Zorgt dat het item netjes in de slot past
        }
    }

    public void ClearSlot()
    {
        currentItemData = null;
        // Leegt de data

        if (currentItemUI != null)
        {
            Destroy(currentItemUI);
            currentItemUI = null;
            // Verwijdert het zichtbare UI item
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        Item draggedItem = eventData.pointerDrag.GetComponent<Item>();

        if (draggedItem == null || draggedItem.itemData == null) return;

        if (draggedItem.OriginInventorySlot == null)
        {
            Debug.Log("Dit item komt niet uit de inventory.");
            return;
        }

        int inventoryIndex = draggedItem.OriginInventorySlot.SlotIndex;
        // Uit welke inventory slot komt het item?

        bool moved = InventorySystem.Instance.MoveInventoryItemToSpecificHotbarSlot(inventoryIndex, hotbarIndex);
        // Verplaatst item in de data van inventory naar hotbar

        if (moved)
        {
            SetItem(draggedItem.itemData);
            // Maakt item zichtbaar in hotbar UI

            draggedItem.MarkDropped();
            // Drop is gelukt

            Destroy(draggedItem.gameObject);
            // Verwijdert het oude inventory UI item

            Debug.Log("Item naar hotbar slot " + (hotbarIndex + 1));
        }
    }
}