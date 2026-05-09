using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public ItemData item; // Het item dat momenteel in deze slot zit
    public bool IsResultSlot; // Alleen aanvinken bij ResultSlot

    [SerializeField] private Image icon; // Child Image waarin het item-plaatje zichtbaar wordt
    [SerializeField] private GameObject itemUIPrefab; // Prefab van sleepbaar UI item

    private GameObject currentItemUI; // Het huidige zichtbare item in deze slot
    private CraftingManager craftingManager; // Verwijzing naar CraftingManager

    private void Start()
    {
        craftingManager = FindFirstObjectByType<CraftingManager>(); // Zoekt CraftingManager in scene
        Clear(); // Maakt slot leeg bij start
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (IsResultSlot) return; // Je mag niks in de ResultSlot droppen

        if (eventData.pointerDrag == null) return; // Als er niks gesleept wordt, stop

        Item draggedItem = eventData.pointerDrag.GetComponent<Item>(); // Pakt Item script van gesleept object

        if (draggedItem == null) return; // Stop als het gesleepte object geen Item script heeft
        if (draggedItem.itemData == null) return; // Stop als het item geen ItemData heeft

        if (draggedItem.OriginSlot != null && draggedItem.OriginSlot.IsResultSlot)
        {
            Debug.Log("Result item mag niet terug in input slots."); // Voorkomt dat output terug input wordt
            return;
        }

        ItemData droppedData = draggedItem.itemData; // Slaat de data op van het gesleepte item

        if (draggedItem.OriginSlot != null)
        {
            draggedItem.OriginSlot.Clear(); // Als het item uit een andere slot kwam, maak die oude slot leeg
        }

        draggedItem.MarkDropped(); // Zegt tegen Item.cs dat drop gelukt is
        Destroy(draggedItem.gameObject); // Verwijdert het oude gesleepte UI object

        SetItem(droppedData); // Zet het item in deze slot

        if (craftingManager != null)
        {
            craftingManager.TryCraft(); // Checkt direct of er een geldig recept is
        }
    }

    public void SetItem(ItemData newItem)
    {
        if (newItem == null)
        {
            Clear(); // Als er geen item is, maak slot leeg
            return;
        }

        item = newItem; // Slaat ItemData op in deze slot

        if (icon != null)
        {
            icon.sprite = newItem.icon; // Zet item-icon in de child Image
            icon.enabled = true; // Maakt icon zichtbaar
            icon.color = Color.white; // Zorgt dat icon niet transparant is
        }

        if (currentItemUI != null)
        {
            Destroy(currentItemUI); // Verwijdert oud zichtbaar item als dat er al was
        }

        if (itemUIPrefab == null)
        {
            Debug.LogError("Item UI Prefab mist op " + gameObject.name); // Waarschuwing als prefab niet ingevuld is
            return;
        }

        currentItemUI = Instantiate(itemUIPrefab, transform); // Maakt nieuw UI item binnen deze slot
        currentItemUI.transform.SetAsLastSibling(); // Zet item bovenop de slot

        Item itemScript = currentItemUI.GetComponent<Item>(); // Pakt Item script van prefab

        if (itemScript != null)
        {
            itemScript.Setup(newItem); // Geeft het nieuwe UI item de juiste data en icon
        }

        RectTransform rt = currentItemUI.GetComponent<RectTransform>(); // Pakt RectTransform van het UI item

        if (rt != null)
        {
            rt.anchorMin = Vector2.zero; // Zet anchor linksonder
            rt.anchorMax = Vector2.one; // Zet anchor rechtsboven
            rt.offsetMin = Vector2.zero; // Geen marge links/onder
            rt.offsetMax = Vector2.zero; // Geen marge rechts/boven
            rt.anchoredPosition = Vector2.zero; // Zet item precies in het midden
            rt.localScale = Vector3.one; // Normale schaal
        }
    }

    public void Clear()
    {
        item = null; // Verwijdert itemdata uit de slot

        if (icon != null)
        {
            icon.sprite = null; // Verwijdert icon
            icon.enabled = false; // Verbergt alleen de child Icon, niet de slot zelf
        }

        if (currentItemUI != null)
        {
            Destroy(currentItemUI); // Verwijdert zichtbaar UI item
            currentItemUI = null; // Reset verwijzing
        }
    }
}

   




