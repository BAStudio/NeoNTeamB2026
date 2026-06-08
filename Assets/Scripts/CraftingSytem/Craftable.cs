

using UnityEngine;

public class Craftable : MonoBehaviour
{
    [SerializeField] private InventoryToggle inventoryToggle; // Object/script waarmee je inventory opent
    [SerializeField] private GameObject craftingUI; // Sleep hier CraftMenu in, niet Canvas
    [SerializeField] private MonoBehaviour playerMovement; // Sleep hier je movement script van Player in

    private bool isPlayerInRange = false; // Checkt of speler bij tafel staat
    private bool isOpen = false; // Checkt of menu open is

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen; // Wisselt open/dicht

            craftingUI.SetActive(isOpen); // Zet CraftMenu aan of uit

            if (isOpen)
            {
                Cursor.lockState = CursorLockMode.None; // Maakt muis vrij
                Cursor.visible = true; // Maakt muis zichtbaar

                if (playerMovement != null)
                    playerMovement.enabled = false; // Zet spelerbeweging uit

                if (inventoryToggle != null)
                    inventoryToggle.OpenInventory(); // Opent inventory
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // Zet muis weer vast
                Cursor.visible = false; // Verbergt muis

                if (playerMovement != null)
                    playerMovement.enabled = true; // Zet spelerbeweging aan

                if (inventoryToggle != null)
                    inventoryToggle.CloseInventory(); // Sluit inventory
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; // Speler staat dichtbij tafel
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // Speler is weg van tafel
            isOpen = false; // Menu-status resetten

            craftingUI.SetActive(false); // CraftMenu sluiten

            Cursor.lockState = CursorLockMode.Locked; // Cursor vastzetten
            Cursor.visible = false; // Cursor verbergen

            if (playerMovement != null)
                playerMovement.enabled = true; // Movement weer aan

            if (inventoryToggle != null)
                inventoryToggle.CloseInventory(); // Inventory sluiten
        }
    }
}
