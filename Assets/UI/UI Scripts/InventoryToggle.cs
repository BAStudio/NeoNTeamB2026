using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    public static bool IsOpen { get; private set; }
    [SerializeField] private Image inventoryBorder;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private GameObject arrow;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
            Time.timeScale = inventoryBorder.enabled ? 0f : 1f;
            Cursor.lockState = inventoryBorder.enabled ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = inventoryBorder.enabled;
        }
        
    }

    private void ToggleInventory()
    {
        if (inventoryBorder != null)
        {
            inventoryBorder.enabled = !inventoryBorder.enabled;
            IsOpen = inventoryBorder.enabled;

            foreach (GameObject slot in slots)
            {
                if (slot != null)
                    slot.SetActive(IsOpen);
            }

            arrow.SetActive(IsOpen);
        }
    }
}