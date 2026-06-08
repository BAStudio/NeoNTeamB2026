using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public ItemData item;
    // Dit is het item dat momenteel in deze crafting slot zit

    public bool IsResultSlot;
    // Zet dit alleen aan bij ResultSlot
    // Slot 1, Slot 2 en Slot 3 moeten dit UIT hebben

    [SerializeField] private Image icon;
    // Hier sleep je de child Icon Image in
    // Dus NIET de Image van Slot zelf, maar Slot1/Icon

    [SerializeField] private GameObject itemUIPrefab;
    // Hier sleep je jouw ItemUI prefab in
    // Dit prefab moet Image + Item.cs + CanvasGroup hebben

    private GameObject currentItemUI;
    // Dit is het zichtbare UI-item dat in deze slot staat

    private CraftingManager craftingManager;
    // Verwijzing naar de CraftingManager

    private void Start()
    {
        craftingManager = FindFirstObjectByType<CraftingManager>();
        // Zoekt de CraftingManager in de scene

        Clear();
        // Maakt de slot leeg bij start
        // Let op: dit zet alleen de Icon uit, niet de hele slot
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (IsResultSlot) return;
        // Je mag niets in de ResultSlot slepen

        if (eventData.pointerDrag == null) return;
        // Stop als er geen item gesleept wordt

        Item draggedItem = eventData.pointerDrag.GetComponent<Item>();
        // Pakt het Item script van het gesleepte UI-object

        if (draggedItem == null) return;
        // Stop als het gesleepte object geen Item script heeft

        if (draggedItem.itemData == null) return;
        // Stop als het item geen ItemData heeft

        if (draggedItem.OriginSlot != null && draggedItem.OriginSlot.IsResultSlot)
        {
            Debug.Log("Result item mag niet terug in crafting input slots.");
            return;
            // Voorkomt dat je Ammo uit ResultSlot terug in Slot 1/2/3 stopt
        }

        ItemData droppedData = draggedItem.itemData;
        // Bewaart welk item je sleept, bijvoorbeeld Metal of GunPowder

        if (draggedItem.OriginInventorySlot != null)
        {
            InventorySystem.Instance.RemoveInventorySlot(draggedItem.OriginInventorySlot.SlotIndex);
            // Als het item uit de inventory komt, verwijder het daar uit de echte inventory
            // Dus Metal verdwijnt uit Inventory Slot1 wanneer je hem naar Craft Slot1 sleept
        }

        if (draggedItem.OriginSlot != null)
        {
            draggedItem.OriginSlot.Clear();
            // Als het item uit een andere crafting slot kwam, maak die oude slot leeg
        }

        draggedItem.MarkDropped();
        // Zegt tegen Item.cs dat de drop gelukt is, dus hij hoeft niet terug te springen

        Destroy(draggedItem.gameObject);
        // Verwijdert het oude gesleepte UI-object

        SetItem(droppedData);
        // Zet het item zichtbaar in deze crafting slot

        if (craftingManager != null)
        {
            craftingManager.TryCraft();
            // Checkt of Slot 1 + Slot 2 + Slot 3 samen een recept vormen
        }
    }

    public void SetItem(ItemData newItem)
    {
        if (newItem == null)
        {
            Clear();
            // Als er geen item wordt meegegeven, maak de slot leeg
            return;
        }

        item = newItem;
        // Slaat de ItemData op in deze slot

        if (icon != null)
        {
            icon.sprite = newItem.icon;
            // Zet het item-plaatje op de child Icon

            icon.enabled = true;
            // Maakt het item-plaatje zichtbaar

            icon.color = Color.white;
            // Zorgt dat het plaatje niet transparant is
        }

        if (currentItemUI != null)
        {
            Destroy(currentItemUI);
            // Verwijdert een oud UI-item als er al iets in deze slot zat
        }

        if (itemUIPrefab == null)
        {
            Debug.LogError("Item UI Prefab mist op " + gameObject.name);
            return;
            // Zonder ItemUI prefab kan er geen sleepbaar item worden gemaakt
        }

        currentItemUI = Instantiate(itemUIPrefab, transform);
        // Maakt een nieuw sleepbaar UI-item binnen deze slot

        currentItemUI.transform.SetAsLastSibling();
        // Zet het item visueel bovenop de slot

        Item itemScript = currentItemUI.GetComponent<Item>();
        // Pakt het Item script van het nieuwe UI-item

        if (itemScript != null)
        {
            itemScript.Setup(newItem);
            // Geeft de juiste ItemData mee aan het UI-item
        }

        Image itemImage = currentItemUI.GetComponent<Image>();
        // Pakt de Image van het nieuwe UI-item

        if (itemImage != null)
        {
            itemImage.sprite = newItem.icon;
            // Zet het juiste plaatje op het UI-item

            itemImage.enabled = true;
            // Maakt het plaatje zichtbaar

            itemImage.color = Color.white;
            // Zorgt dat het item niet transparant is
        }

        RectTransform rt = currentItemUI.GetComponent<RectTransform>();
        // Pakt de RectTransform zodat het item netjes in de slot past

        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            // Anchor links/onder

            rt.anchorMax = Vector2.one;
            // Anchor rechts/boven

            rt.offsetMin = Vector2.zero;
            // Geen marge links/onder

            rt.offsetMax = Vector2.zero;
            // Geen marge rechts/boven

            rt.anchoredPosition = Vector2.zero;
            // Zet item exact in het midden

            rt.localScale = Vector3.one;
            // Normale schaal
        }
    }

    public void Clear()
    {
        item = null;
        // Verwijdert de ItemData uit deze slot

        if (icon != null)
        {
            icon.sprite = null;
            // Verwijdert het plaatje uit de child Icon

            icon.enabled = false;
            // Verbergt alleen de child Icon
            // Niet de slot zelf
        }

        if (currentItemUI != null)
        {
            Destroy(currentItemUI);
            // Verwijdert het zichtbare sleepbare UI-item

            currentItemUI = null;
            // Reset de verwijzing
        }
    }
}

   




