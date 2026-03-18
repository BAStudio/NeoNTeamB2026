using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public ItemData item;
    [SerializeField] private Image icon;
    [SerializeField] private bool IsResultSlot;

    private CraftingManager craftingManager;

    private void Start()
    {
        craftingManager = FindFirstObjectByType<CraftingManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("resultshow");
        if (IsResultSlot) return;
        Item droppedItem = eventData.pointerDrag.GetComponent<Item>();

        if (droppedItem != null)
        { 
            SetItem(droppedItem.itemData);

            Destroy(droppedItem.gameObject);

            craftingManager.TryCraft();
        }
    }

    public void SetItem(ItemData newItem)
    {
        item = newItem;
        gameObject.SetActive(true);
        Debug.Log("ICON: " + newItem.icon);
        icon.sprite = newItem.icon;
        icon.enabled = true;
    }

    public void Clear()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }
}
   




