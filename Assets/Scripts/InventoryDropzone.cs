using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Nodig om het inventory icoon te tonen

public class InventoryDropzone : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotIndex;
    // Dit is de visuele inventory-slot:
    // Slot1 = 0
    // Slot2 = 1
    // Slot3 = 2
    // ...
    // Slot12 = 11

    [SerializeField] private Image icon;
    // Hier sleep je de child Icon Image van deze inventory slot in
    // Bijvoorbeeld Slot9/Icon

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        // Stop als er niks wordt gesleept

        Item draggedItem = eventData.pointerDrag.GetComponent<Item>();
        // Pakt het Item script van het gesleepte UI item

        if (draggedItem == null) return;
        // Stop als het gesleepte object geen Item script heeft

        ItemData itemData = draggedItem.itemData;
        // Pakt de ItemData van het gesleepte item, bijvoorbeeld Ammo

        if (itemData == null) return;
        // Stop als ItemData leeg is

        if (itemData.prefab == null)
        {
            Debug.LogError("Geen prefab gekoppeld aan item: " + itemData.itemName);
            return;
            // Zonder prefab kan het item niet echt in de inventory worden gezet
        }

        GameObject newItem = Instantiate(itemData.prefab);
        // Maakt een echt GameObject aan voor je inventory

        ItemPickup pickup = newItem.GetComponent<ItemPickup>();
        // Pakt ItemPickup van de prefab als die bestaat

        if (pickup != null)
        {
            pickup.itemData = itemData;
            // Geeft de juiste ItemData mee

            pickup.itemID = itemData.itemName;
            // Zet itemID gelijk aan itemName
        }

        bool added = InventorySystem.Instance.AddItemToInventorySlot(slotIndex, newItem);
        // BELANGRIJK:
        // Dit voegt toe aan inventory index 5 t/m 16
        // Dus niet aan hotbar index 0 t/m 4

        if (added)
        {
            if (icon != null)
            {
                icon.sprite = itemData.icon;
                // Zet het plaatje in de inventory UI slot

                icon.enabled = true;
                // Maakt het icoon zichtbaar

                icon.color = Color.white;
                // Zorgt dat het icoon niet transparant is
            }
            else
            {
                Debug.LogWarning("Icon is niet ingevuld op: " + gameObject.name);
                // Waarschuwing als je Icon niet hebt gekoppeld
            }

            Slot parentSlot = draggedItem.OriginSlot;
            // Kijkt uit welke crafting slot dit item kwam

            if (parentSlot != null && parentSlot.IsResultSlot)
            {
                CraftingManager cm = FindFirstObjectByType<CraftingManager>();
                // Zoekt CraftingManager

                if (cm != null)
                {
                    cm.ConsumeIngredients();
                    // Verbruikt de ingrediënten uit de crafting slots
                }
            }

            draggedItem.MarkDropped();
            // Zegt tegen Item.cs dat de drop succesvol was

            Destroy(draggedItem.gameObject);
            // Verwijdert het gesleepte UI item

            Debug.Log(itemData.itemName + " toegevoegd aan inventory slot " + (slotIndex + 1));
            // Console melding
        }
        else
        {
            Destroy(newItem);
            // Als toevoegen mislukt, verwijdert hij het aangemaakte item

            Debug.Log("Inventory slot " + (slotIndex + 1) + " is al bezet.");
            // Console melding
        }
    }
}
