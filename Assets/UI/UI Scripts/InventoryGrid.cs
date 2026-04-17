using UnityEngine;
using TMPro;

public class InventoryGrid : MonoBehaviour
{
    UICharInfo[] gridSlots;
    UICharInfo[] inventorySlots;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridSlots = GetComponentsInChildren<UICharInfo>();
       if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.OnInventoryChanged += UpdateGrid;
            UpdateGrid();
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Onclick()
    {
       


    }

    private void UpdateGrid()
    {
     
    }
}
