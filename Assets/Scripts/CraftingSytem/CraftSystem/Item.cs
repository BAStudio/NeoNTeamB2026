using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData; // Welk item dit is, bijvoorbeeld Metal of GunPowder

    public Slot OriginSlot { get; private set; } // Als item uit crafting slot komt
    public InventoryDropzone OriginInventorySlot { get; private set; } // Als item uit inventory komt

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private Transform originalParent;
    private Vector2 originalPosition;
    private bool wasDropped = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        UpdateIcon();
    }

    public void Setup(ItemData data)
    {
        itemData = data; // Koppelt dit UI item aan Metal/GunPowder/Ammo
        UpdateIcon(); // Zet meteen het juiste plaatje
    }

    public void SetOriginInventorySlot(InventoryDropzone inventorySlot)
    {
        OriginInventorySlot = inventorySlot; // Onthoudt uit welke inventory-slot dit item kwam
    }

    private void UpdateIcon()
    {
        Image image = GetComponent<Image>();

        if (image != null && itemData != null)
        {
            image.sprite = itemData.icon;
            image.enabled = true;
            image.color = Color.white;
        }
    }

    public void MarkDropped()
    {
        wasDropped = true; // Drop is gelukt, dus item hoeft niet terug te springen
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas == null) return;

        wasDropped = false;

        OriginSlot = GetComponentInParent<Slot>(); // Checkt of item uit craft slot komt

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform); // Zet item tijdelijk boven alles
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false; // Belangrijk zodat dropzones geraakt worden
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (wasDropped) return;

        transform.SetParent(originalParent); // Als drop mislukt, terug naar oude plek
        rectTransform.anchoredPosition = originalPosition;
    }
}
