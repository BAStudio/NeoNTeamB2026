using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        canvasGroup.blocksRaycasts  = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        //  Check waar je dropt
        if (eventData.pointerEnter != null)
        {
            Slot slot = eventData.pointerEnter.GetComponent<Slot>();

            //  Als je op inventory slot dropt
            if (slot != null && !slot.IsResultSlot)
            {
                //  maak wereld item
                GameObject newItem = Instantiate(itemData.prefab);

                ItemPickup pickup = newItem.GetComponent<ItemPickup>();
                if (pickup != null)
                {
                    pickup.itemData = itemData;
                }

                bool added = InventorySystem.Instance.AddItem(newItem);

                if (added)
                {
                    //  check of dit uit result slot komt
                    Slot parentSlot = GetComponentInParent<Slot>();

                    if (parentSlot != null && parentSlot.IsResultSlot)
                    {
                        CraftingManager cm = FindFirstObjectByType<CraftingManager>();
                        cm.ConsumeIngredients();
                        parentSlot.Clear();
                    }

                    Destroy(gameObject);
                    return;
                }
            }
        }

        //  anders terug naar start
        transform.position = startPosition;
    }
}
