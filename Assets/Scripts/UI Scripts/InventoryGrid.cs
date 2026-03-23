using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGrid : MonoBehaviour
{ 
    [SerializeField] private RectTransform[] slots;
    [SerializeField] private int columns = 3;
    private int currentIndex = 0;

    private void Start()
    {
        SelectSlot(0);
    }

    private void Update()
    {
        if (!InventoryToggle.IsOpen)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(1, 0);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(-1, 0);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(0, 1);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Move(0, -1);

        if (Input.GetKeyDown(KeyCode.Return))
            EquipSelected();


        Debug.Log("Current Index: " + currentIndex);
    }
    
    void Move(int x, int y) 
    {
        int row = currentIndex / columns;
        int col = currentIndex % columns;

        col += x;
        row += y;

        int newIndex = row * columns + col;

        if (newIndex >= 0 && newIndex < slots.Length && col >= 0 && col < columns)
        {
            SelectSlot(newIndex);
        }

    }

    void SelectSlot(int index)
    {
        currentIndex = index;
        EventSystem.current.SetSelectedGameObject(slots[currentIndex].gameObject);

        if (InventorySystem.Instance != null)
            InventorySystem.Instance.ChangeSlot(currentIndex);
    }

    void EquipSelected()
    {
        if (InventorySystem.Instance == null) return;

        GameObject item = InventorySystem.Instance.GetItemInSlot(currentIndex);
        if (item != null)
        {
            InventorySystem.Instance.ChangeSlot(currentIndex);
            Debug.Log("Equipped item from slot " + (currentIndex + 1));
        }
    }
}
