using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGrid : MonoBehaviour
{
    [SerializeField] private RectTransform[] slots;
    // Hier sleep je je Inventory Slot1 t/m Slot12 in

    [SerializeField] private int columns = 3;
    // Hoeveel kolommen je inventory heeft
    // Bijvoorbeeld 3 kolommen bij 12 slots = 4 rijen

    private int currentIndex = 0;
    // Houdt bij welke inventory-slot geselecteerd is

    private void Start()
    {
        SelectSlot(0);
        // Selecteert automatisch de eerste inventory-slot bij start
    }

    private void Update()
    {
        if (!InventoryToggle.IsOpen)
            return;
        // Alleen navigeren als inventory open is

        if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(1, 0);
        // Naar rechts bewegen

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(-1, 0);
        // Naar links bewegen

        if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(0, 1);
        // Naar beneden bewegen

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Move(0, -1);
        // Naar boven bewegen

        if (Input.GetKeyDown(KeyCode.Return))
            EquipSelected();
        // Enter probeert het geselecteerde inventory-item te equippen
    }

    void Move(int x, int y)
    {
        int row = currentIndex / columns;
        // Berekent de huidige rij

        int col = currentIndex % columns;
        // Berekent de huidige kolom

        col += x;
        // Past horizontale beweging toe

        row += y;
        // Past verticale beweging toe

        int newIndex = row * columns + col;
        // Berekent de nieuwe index

        if (newIndex >= 0 && newIndex < slots.Length && col >= 0 && col < columns)
        {
            SelectSlot(newIndex);
            // Selecteert alleen als de nieuwe index geldig is
        }
    }

    void SelectSlot(int index)
    {
        currentIndex = index;
        // Slaat geselecteerde inventory-slot op

        if (EventSystem.current != null && slots[currentIndex] != null)
        {
            EventSystem.current.SetSelectedGameObject(slots[currentIndex].gameObject);
            // Selecteert de UI slot visueel
        }

        Debug.Log("Selected inventory slot: " + (currentIndex + 1));
        // Laat zien welke inventory-slot geselecteerd is
    }

    void EquipSelected()
    {
        if (InventorySystem.Instance == null) return;
        // Stop als InventorySystem mist

        GameObject item = InventorySystem.Instance.GetItemInInventorySlot(currentIndex);
        // Pakt item uit inventory slot
        // currentIndex 0 = Inventory Slot1
        // currentIndex 1 = Inventory Slot2

        if (item != null)
        {
            InventorySystem.Instance.MoveInventoryItemToHotbar(currentIndex);
            // Verplaatst geselecteerd inventory-item naar eerste vrije hotbar slot

            Debug.Log("Equipped item from inventory slot " + (currentIndex + 1));
        }
        else
        {
            Debug.Log("Geen item in inventory slot " + (currentIndex + 1));
        }
    }
}
