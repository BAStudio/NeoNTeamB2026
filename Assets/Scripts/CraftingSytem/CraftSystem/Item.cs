using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData; // Hier staat welk item dit is, bijvoorbeeld Metal of GunPowder

    public Slot OriginSlot { get; private set; } // Onthoudt uit welke slot dit item kwam

    private RectTransform rectTransform; // Hiermee kunnen we het UI item verplaatsen
    private CanvasGroup canvasGroup; // Nodig zodat het item tijdens slepen geen raycast blokkeert
    private Canvas canvas; // De Canvas waarin dit UI item staat

    private Transform originalParent; // Onthoudt waar het item stond vóór het slepen
    private Vector2 originalPosition; // Onthoudt de positie vóór het slepen
    private bool wasDropped = false; // Wordt true als het item succesvol ergens is gedropt

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // Pakt de RectTransform van dit UI object
        canvasGroup = GetComponent<CanvasGroup>(); // Pakt de CanvasGroup als die bestaat
        canvas = GetComponentInParent<Canvas>(); // Zoekt de Canvas boven dit object

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>(); // Maakt CanvasGroup aan als die mist

        Image image = GetComponent<Image>(); // Pakt de Image component

        if (image != null && itemData != null)
        {
            image.sprite = itemData.icon; // Zet de juiste sprite/icon op het item
            image.enabled = true; // Zorgt dat het plaatje zichtbaar is
            image.color = Color.white; // Zorgt dat de afbeelding niet transparant/zwart is
        }
    }

    public void Setup(ItemData data)
    {
        itemData = data; // Geeft dit UI item de juiste ItemData

        Image image = GetComponent<Image>(); // Pakt opnieuw de Image component

        if (image != null && itemData != null)
        {
            image.sprite = itemData.icon; // Zet het juiste icoon
            image.enabled = true; // Zet de afbeelding aan
            image.color = Color.white; // Zorgt dat het icoon zichtbaar blijft
        }
    }

    public void MarkDropped()
    {
        wasDropped = true; // Laat weten dat dit item succesvol is gedropt
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>(); // Zoekt Canvas opnieuw als die nog leeg is

        if (canvas == null) return; // Zonder Canvas kunnen we niet goed slepen

        wasDropped = false; // Reset drop-status bij nieuw slepen

        OriginSlot = GetComponentInParent<Slot>(); // Checkt of dit item uit een crafting slot komt

        originalParent = transform.parent; // Onthoudt waar het item vandaan kwam
        originalPosition = rectTransform.anchoredPosition; // Onthoudt de oude positie

        transform.SetParent(canvas.transform); // Zet item tijdelijk direct onder Canvas zodat het vrij kan bewegen
        transform.SetAsLastSibling(); // Zet item helemaal vooraan zodat je het ziet tijdens slepen

        canvasGroup.blocksRaycasts = false; // Belangrijk: hierdoor kan de slot onder je muis de drop ontvangen

        if (itemData != null)
            Debug.Log("Begin drag: " + itemData.itemName); // Testmelding in Console
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return; // Zonder Canvas niks doen

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // Laat item met muis meebewegen
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // Zet raycasts weer aan na het slepen

        if (wasDropped) return; // Als item succesvol gedropt is, hoeft hij niet terug

        transform.SetParent(originalParent); // Als drop mislukt, zet item terug naar oude parent
        rectTransform.anchoredPosition = originalPosition; // Zet item terug naar oude positie
    }
}
