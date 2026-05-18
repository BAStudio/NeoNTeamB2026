using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData;
    // Dit is de data van het item, bijvoorbeeld Pistol, Ammo, Metal

    public Slot OriginSlot { get; private set; }
    // Als het item uit een crafting slot komt

    public InventoryDropzone OriginInventorySlot { get; private set; }
    // Als het item uit de inventory komt

    public HotbarDropzone OriginHotbarSlot { get; private set; }
    // Als het item uit de hotbar komt

    private RectTransform rectTransform;
    // Nodig om het UI item te bewegen

    private CanvasGroup canvasGroup;
    // Nodig zodat dropzones het item kunnen ontvangen

    private Canvas canvas;
    // De canvas waarin het item beweegt

    private Transform originalParent;
    // Onthoudt waar het item vandaan kwam

    private Vector2 originalPosition;
    // Onthoudt de originele positie

    private bool wasDropped = false;
    // Checkt of de drop gelukt is

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // Pakt de RectTransform van dit UI item

        canvasGroup = GetComponent<CanvasGroup>();
        // Pakt CanvasGroup

        canvas = GetComponentInParent<Canvas>();
        // Zoekt de canvas waar dit item in zit

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            // Maakt CanvasGroup aan als die mist
        }

        UpdateIcon();
        // Zet het juiste plaatje
    }

    public void Setup(ItemData data)
    {
        itemData = data;
        // Zet de juiste ItemData

        UpdateIcon();
        // Update het icoon
    }

    public void SetOriginInventorySlot(InventoryDropzone inventorySlot)
    {
        OriginInventorySlot = inventorySlot;
        // Onthoudt uit welke inventory slot dit item komt
    }

    public void SetOriginHotbarSlot(HotbarDropzone hotbarSlot)
    {
        OriginHotbarSlot = hotbarSlot;
        // Onthoudt uit welke hotbar slot dit item komt
    }

    private void UpdateIcon()
    {
        Image image = GetComponent<Image>();
        // Pakt de Image component

        if (image != null && itemData != null)
        {
            image.sprite = itemData.icon;
            // Zet het item icoon

            image.enabled = true;
            // Zet image aan

            image.color = Color.white;
            // Zorgt dat hij zichtbaar is
        }
    }

    public void MarkDropped()
    {
        wasDropped = true;
        // Wordt aangeroepen als drop gelukt is
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
            // Zoekt canvas opnieuw als die leeg is
        }

        if (canvas == null) return;
        // Zonder canvas kan je niet slepen

        wasDropped = false;
        // Reset drop status

        OriginSlot = GetComponentInParent<Slot>();
        // Checkt of item uit crafting komt

        OriginInventorySlot = GetComponentInParent<InventoryDropzone>();
        // Checkt of item uit inventory komt

        OriginHotbarSlot = GetComponentInParent<HotbarDropzone>();
        // Checkt of item uit hotbar komt

        originalParent = transform.parent;
        // Onthoudt de oude parent

        originalPosition = rectTransform.anchoredPosition;
        // Onthoudt de oude positie

        transform.SetParent(canvas.transform);
        // Zet item tijdelijk boven alles in de canvas

        transform.SetAsLastSibling();
        // Zet item visueel bovenaan

        canvasGroup.blocksRaycasts = false;
        // Hierdoor kan de dropzone onder je muis de drop ontvangen
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        // Stop als canvas mist

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // Laat item meebewegen met de muis
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        // Zet raycasts weer aan

        if (wasDropped) return;
        // Als drop gelukt is, hoeft item niet terug

        transform.SetParent(originalParent);
        // Zet item terug naar oude parent

        rectTransform.anchoredPosition = originalPosition;
        // Zet item terug naar oude plek
    }
}