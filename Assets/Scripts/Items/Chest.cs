using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest Settings")]
    [SerializeField] private ItemActivatorData itemActivatorData;
    [SerializeField] private string interactTag = "Player";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Dialogue")]
    [SerializeField] private string chestID;         
    [SerializeField] private string failDialogueID;  

    private bool playerInRange;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (InventorySystem.Instance.HasItem(itemActivatorData.itemName))
            {
                OpenChest();

                
                GameEvents.OnDialogueStart?.Invoke(chestID);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("You need the " + itemActivatorData.itemName + " to open this!");

                
                if (!string.IsNullOrEmpty(failDialogueID))
                {
                    GameEvents.OnDialogueStart?.Invoke(failDialogueID);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(interactTag))
        {
            playerInRange = true;
            Debug.Log("Press " + interactKey + " to open chest");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(interactTag))
        {
            playerInRange = false;
        }
    }

    private void OpenChest()
    {
        Instantiate(
            itemActivatorData.RewardItem,
            transform.position + Vector3.up,
            Quaternion.identity
        );
    }
}