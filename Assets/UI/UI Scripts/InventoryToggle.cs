using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private Image inventoryBorder; // Lowercase 'i' is standard for private fields

    void Start()
    {
        // Try to get the image if it wasn't assigned in the Inspector
        if (inventoryBorder == null)
        {
            inventoryBorder = GetComponent<Image>();
        }

        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
            Time.timeScale = inventoryBorder.enabled ? 0f : 1f; // Pause the game when inventory is open, resume when closed
            Cursor.lockState = inventoryBorder.enabled ? CursorLockMode.None : CursorLockMode.Locked; // Unlock cursor when inventory is open, lock it when closed
        }
    }

    private void ToggleInventory()
    {
        if (inventoryBorder != null)
        {
            // This flips the true/false state (if it's on, it turns off, and vice versa)
            inventoryBorder.enabled = !inventoryBorder.enabled;
        }
    }
}