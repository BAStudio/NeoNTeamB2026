

using UnityEngine;

public class Craftable : MonoBehaviour
{
    [SerializeField] private InventoryToggle inventoryToggle;
    [SerializeField] GameObject craftingUI;
    [SerializeField] MonoBehaviour playerMovement; // sleep je movement script hier

    private bool isPlayerInRange = false;
    private bool isOpen = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;

            craftingUI.SetActive(isOpen);

            if (isOpen)
            {
                // UI open  cursor vrij + player uit
                Cursor.lockState = CursorLockMode.None;//je kan de muis vrij bewgen
                Cursor.visible = true;

                playerMovement.enabled = false;
                inventoryToggle.OpenInventory();
            }
            else
            {
                // UI dicht  cursor lock + player aan
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                playerMovement.enabled = true;
                inventoryToggle.CloseInventory();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            craftingUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerMovement.enabled = true;
        }
    }
}
