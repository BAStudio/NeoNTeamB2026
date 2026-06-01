using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemDrag : InventorySystem, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Item Drag")]
    public Image image;

    [HideInInspector] public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
       image.raycastTarget = false;
       parentAfterDrag = transform.parent;
       transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       image.raycastTarget = true;
       image.transform.SetParent(parentAfterDrag);
    }
}
