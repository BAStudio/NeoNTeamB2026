using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    void Update()
    {
        if (InventoryToggle.IsOpen)
            return;

        if (InventorySystem.Instance == null)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) InventorySystem.Instance.ChangeSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) InventorySystem.Instance.ChangeSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) InventorySystem.Instance.ChangeSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) InventorySystem.Instance.ChangeSlot(3);
    }
}
