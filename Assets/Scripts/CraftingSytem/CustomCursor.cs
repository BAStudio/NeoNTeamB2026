using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Image cursorImage;
    // De afbeelding die aan je cursor hangt

    private ItemData heldItem;
    // Het item dat de cursor vasthoudt

    void Update()
    {
        transform.position = Input.mousePosition;
        // Zet de cursor UI op de muispositie
    }

    public void SetCursor(ItemData item)
    {
        heldItem = item;
        // Slaat het item op

        cursorImage.sprite = item.icon;
        // Zet de juiste icon

        cursorImage.enabled = true;
        // Maakt de cursor icon zichtbaar
    }

    public ItemData GetHeldItem()
    {
        return heldItem;
        // Geeft terug welk item de cursor vasthoudt
    }

    public void ClearCursor()
    {
        heldItem = null;
        // Leegt het item

        cursorImage.enabled = false;
        // Verbergt de cursor icon
    }
}


