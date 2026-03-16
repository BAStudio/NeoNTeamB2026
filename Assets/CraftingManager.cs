using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private Slot[] craftingSlots;
    [SerializeField] private RecipeData[] recipes;

    private int currentSlot = 0;

    public void AddItem(ItemData item)
    {
        if (currentSlot >= craftingSlots.Length)
            return;

        craftingSlots[currentSlot].SetItem(item);
        currentSlot++;
    }

    public ItemData CheckRecipe()
    {
        List<ItemData> playerItems = new List<ItemData>();

        foreach (Slot slot in craftingSlots)
        {
            if (slot.item != null)
                playerItems.Add(slot.item);
        }

        playerItems.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        foreach (RecipeData recipe in recipes)
        {
            List<ItemData> recipeItems = new List<ItemData>(recipe.ingredients);
            recipeItems.Sort((a, b) => a.itemName.CompareTo(b.itemName));

            if (recipeItems.Count != playerItems.Count)
                continue;

            bool match = true;

            for (int i = 0; i < playerItems.Count; i++)
            {
                if (playerItems[i] != recipeItems[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return recipe.result;
        }

        return null;
    }
}

