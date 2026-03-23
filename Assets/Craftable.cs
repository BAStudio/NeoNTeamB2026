

using UnityEngine;

public class Craftable : MonoBehaviour
{
    [SerializeField] GameObject CraftingUI;
    private bool isPlayerInRange = false;

    private void Update()
    {

        if(isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CraftingUI.SetActive(CraftingUI.activeSelf);
        }
        


    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered: " + other.name);
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            CraftingUI.SetActive(false);
        }

    }
}
