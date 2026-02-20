using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    private List<string> items = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log("Added to inventory: " + itemName);
        PrintInventory();
    }

    public void RemoveItem(string itemName)
    {
        if (items.Contains(itemName))
        {
            items.Remove(itemName);
            Debug.Log("Removed from inventory: " + itemName);
            PrintInventory();
        }
    }

    void PrintInventory()
    {
        Debug.Log("=== INVENTORY ===");
        foreach (string item in items)
        {
            Debug.Log(item);
        }
    }
}