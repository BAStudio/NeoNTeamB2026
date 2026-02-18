using Unity.VisualScripting;
using UnityEngine;

public class Itempickup : MonoBehaviour
{
    public string itemName = "Item";
    private bool playerInRange = false;


    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
            
        {
            PickUp();
        }
    }

    void PickUp()
    {
        Debug.Log("Picked up:" + itemName);
        

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }


}


