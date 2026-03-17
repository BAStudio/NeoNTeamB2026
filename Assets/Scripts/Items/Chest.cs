using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private ItemActivatorData itemActivatorData;
    [SerializeField] private string interactTag = "Player";
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private GameObject RequiredItem;

    private bool playerInRange;
    private GameObject promptInstance;

    void Update()
    {
        //checks if the player is in range and presses the interact key
        // checks if the player has the required item to open the chest
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            // Use the string name from your ScriptableObject to check the inventory
            if (InventorySystem.Instance.HasItem(itemActivatorData.itemName))
            {
                OpenChest();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("You need the " + itemActivatorData.itemName + " to open this!");
            }
        }



    }

    private void OpenChest()
    {
        // Removes the chest and spawns the item 
        Instantiate(itemActivatorData.RewardItem, transform.position + Vector3.up, Quaternion.identity);
    }
}