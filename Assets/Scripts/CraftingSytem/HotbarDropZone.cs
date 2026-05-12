using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private int hotbarIndex;
    // Hotbar Slot 1 = 0
    // Hotbar Slot 2 = 1
    // Hotbar Slot 3 = 2
    // Hotbar Slot 4 = 3
    // Hotbar Slot 5 = 4

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        // Stop als er niks wordt gesleept

        Item draggedItem = eventData.pointerDrag.GetComponent<Item>();
        // Pakt het gesleepte Item script

        if (draggedItem == null) return;
        // Stop als het geen item is

        if (draggedItem.OriginInventorySlot == null)
        {
            Debug.Log("Dit item komt niet uit de inventory.");
            return;
            // We verplaatsen alleen items vanuit inventory naar hotbar
        }

        int inventoryIndex = draggedItem.OriginInventorySlot.SlotIndex;
        // Kijkt uit welke inventory slot het item komt

        bool moved = InventorySystem.Instance.MoveInventoryItemToSpecificHotbarSlot(inventoryIndex, hotbarIndex);
        // Verplaatst item naar deze specifieke hotbar slot

        if (moved)
        {
            draggedItem.MarkDropped();
            // Zegt dat de drag/drop gelukt is

            Destroy(draggedItem.gameObject);
            // Verwijdert het UI item dat je sleepte

            Debug.Log("Item verplaatst naar hotbar slot " + (hotbarIndex + 1));
        }
    }
}