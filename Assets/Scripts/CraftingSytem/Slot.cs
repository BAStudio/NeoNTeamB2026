using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public ItemData item;
    public bool IsResultSlot; //  moet public zijn voor Item.cs

    [SerializeField] private Image icon;
    [SerializeField] private GameObject itemUIPrefab;

    private GameObject currentItemUI;
    private CraftingManager craftingManager;

    private void Start()
    {
        craftingManager = FindFirstObjectByType<CraftingManager>();
        Debug.Log("CraftingManager found: " + craftingManager);
    }

    public void OnDrop(PointerEventData eventData)
    {
        //  Result slot mag geen input accepteren
        if (IsResultSlot) return;

        Item droppedItem = eventData.pointerDrag.GetComponent<Item>();

        if (droppedItem == null) return;

        //  zet item in slot
        SetItem(droppedItem.itemData);

        //  verwijder oude UI item
        Destroy(droppedItem.gameObject);

        //  check crafting
        craftingManager.TryCraft();
    }

    public void SetItem(ItemData newItem)
    {
        item = newItem;

        icon.sprite = newItem.icon;
        icon.enabled = true;

        if (currentItemUI != null)
            Destroy(currentItemUI);

        currentItemUI = Instantiate(itemUIPrefab, transform);

        Item itemScript = currentItemUI.GetComponent<Item>();
        itemScript.itemData = newItem;

        //  NIEUW (DIT MIS JE)
        Image itemImage = currentItemUI.GetComponent<Image>();
        itemImage.sprite = newItem.icon;
    }
    public void Clear()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;

        if (currentItemUI != null)
            Destroy(currentItemUI);
    }
}

   




