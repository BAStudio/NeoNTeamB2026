using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    void Update()
    {
        if (InventoryToggle.IsOpen)
            return;

        if (InventorySystem.Instance == null)
            return;

        // 1-4 keys are now handled by InventoryQuickaccess.cs
    }
}
