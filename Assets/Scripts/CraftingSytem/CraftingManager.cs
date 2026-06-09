using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private Slot[] craftingSlots; // Hier komen Slot 1, Slot 2 en Slot 3 in
    [SerializeField] private Slot resultSlot; // Hier komt ResultSlot in
    [SerializeField] private RecipeData[] recipes; // Hier komen je recepten in, bijvoorbeeld AmmoRecipe

    public void TryCraft()
    {
        foreach (RecipeData recipe in recipes)
        {
            if (RecipeMatches(recipe))
            {
                resultSlot.SetItem(recipe.result); // Als recept klopt, zet resultaat in ResultSlot
                Debug.Log("Crafting gelukt: " + recipe.result.itemName); // Console melding
                return; // Stop, want recept is gevonden
            }
        }

        resultSlot.Clear(); // Als geen recept klopt, maak ResultSlot leeg
        Debug.Log("Geen geldig recept gevonden."); // Console melding
    }

    private bool RecipeMatches(RecipeData recipe)
    {
        if (recipe == null) return false; // Geen recept betekent geen match
        if (recipe.ingredients == null) return false; // Geen ingrediënten betekent geen match
        if (recipe.result == null) return false; // Geen resultaat betekent geen geldig recept

        if (recipe.ingredients.Length > craftingSlots.Length)
            return false; // Recept kan niet meer ingrediënten hebben dan slots

        bool[] usedSlots = new bool[craftingSlots.Length]; // Houdt bij welke slots al gebruikt zijn

        foreach (ItemData ingredient in recipe.ingredients)
        {
            bool foundIngredient = false; // Wordt true als dit ingrediënt gevonden is

            for (int i = 0; i < craftingSlots.Length; i++)
            {
                if (usedSlots[i]) continue; // Sla slot over als die al gebruikt is

                if (craftingSlots[i].item == ingredient)
                {
                    usedSlots[i] = true; // Markeer deze slot als gebruikt
                    foundIngredient = true; // Ingrediënt gevonden
                    break; // Stop zoeken naar dit ingrediënt
                }
            }

            if (!foundIngredient)
                return false; // Als één ingrediënt ontbreekt, klopt recept niet
        }

        int filledSlotCount = 0; // Telt hoeveel slots gevuld zijn

        for (int i = 0; i < craftingSlots.Length; i++)
        {
            if (craftingSlots[i].item != null)
                filledSlotCount++; // Tel gevulde slot
        }

        return filledSlotCount == recipe.ingredients.Length; // Recept klopt alleen zonder extra items
    }

    public void ConsumeIngredients()
    {
        foreach (Slot slot in craftingSlots)
        {
            slot.Clear(); // Maakt input slots leeg na het pakken van resultaat
        }

        resultSlot.Clear(); // Maakt result slot leeg
    }

    public void ClearCraftingSlots()
    {
        foreach (Slot slot in craftingSlots)
        {
            slot.Clear(); // Maakt alle input slots leeg
        }

        resultSlot.Clear(); // Maakt output leeg
    }

}

