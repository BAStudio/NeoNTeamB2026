using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDropzone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        // Checkt of je echt iets aan het slepen bent

        Item draggedItem = eventData.pointerDrag.GetComponent<Item>();
        // Pakt het Item script van het gesleepte object

        if (draggedItem == null) return;
        // Als het object geen Item script heeft, stopt de code

        ItemData itemData = draggedItem.itemData;
        // Pakt de data van het item, bijvoorbeeld Ammo

        if (itemData == null) return;
        // Als er geen ItemData is, stopt de code

        if (itemData.prefab == null)
        {
            Debug.LogError("Geen prefab gekoppeld aan item: " + itemData.itemName);
            return;
            // Zonder prefab kan het item niet in de inventory worden gezet
        }

        GameObject newItem = Instantiate(itemData.prefab);
        // Maakt een echt GameObject aan van de Ammo prefab

        ItemPickup pickup = newItem.GetComponent<ItemPickup>();
        // Checkt of de prefab een ItemPickup script heeft

        if (pickup != null)
        {
            pickup.itemData = itemData;
            // Geeft de juiste ItemData mee aan het inventory item

            pickup.itemID = itemData.itemName;
            // Zet het ID gelijk aan de itemnaam
        }

        bool added = InventorySystem.Instance.AddItem(newItem);
        // Probeert het item toe te voegen aan je inventory

        if (added)
        {
            Slot parentSlot = draggedItem.OriginSlot;
            // Kijkt uit welke slot dit item kwam

            if (parentSlot != null && parentSlot.IsResultSlot)
            {
                CraftingManager cm = FindFirstObjectByType<CraftingManager>();
                // Zoekt de CraftingManager

                if (cm != null)
                {
                    cm.ConsumeIngredients();
                    // Verwijdert Metal en GunPowder uit de crafting slots
                }
            }

            draggedItem.MarkDropped();
            // Zegt tegen Item.cs dat de drop gelukt is

            Destroy(draggedItem.gameObject);
            // Verwijdert het UI-plaatje dat je hebt gesleept

            Debug.Log("Item toegevoegd aan inventory: " + itemData.itemName);
            // Laat zien dat het gelukt is
        }
        else
        {
            Destroy(newItem);
            // Als inventory vol is, verwijder het gemaakte item

            Debug.Log("Inventory is vol.");
            // Console melding
        }
    }
}
