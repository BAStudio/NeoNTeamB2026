using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Image cursorImage;

    private ItemData heldItem;

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetCursor(ItemData item)
    {
        heldItem = item;
        cursorImage.sprite = item.icon;
        cursorImage.enabled = true;
    }

    public ItemData GetHeldItem()
    {
        return heldItem;
    }

    public void ClearCursor()
    {
        heldItem = null;
        cursorImage.enabled = false;
    }
}


