using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : Item
{
    private Item _currentItem;
   [SerializeField] private Image _customCursor;

  [SerializeField]  private Slot[] craftingSlots;
    
   [SerializeField] List <Item> itemList;
    [SerializeField] string[] recipes;
    [SerializeField] Item[] recipeResults;
    [SerializeField] Slot[] slotsResults;

    private void Start()
    {
        void Start()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i] = null;
            }
        }
    }
    private void Update() // Draait elke frame
    {
        if (_currentItem != null) // Als speler een item vasthoudt
        {
            _customCursor.transform.position = Input.mousePosition; // Cursor volgt muis
        }

        if (Input.GetMouseButtonUp(0)) // Als linker muisknop wordt losgelaten
        {
            if (_currentItem != null) // Als speler een item vasthoudt
            {
                _customCursor.gameObject.SetActive(false); // Verberg cursor icoon

                Slot nearestSlot = null; // Variabele voor dichtstbijzijnde slot
                float shortestDistance = float.MaxValue; // Start met grootste afstand

                foreach (Slot slot in craftingSlots) // Loop door alle slots
                {
                    float distance = Vector2.Distance(Input.mousePosition, slot.transform.position); // Bereken afstand muis  slot

                    if (distance < shortestDistance) // Als dit slot dichterbij is
                    {
                        shortestDistance = distance; // Sla nieuwe kortste afstand op
                        nearestSlot = slot; // Dit slot is nu het dichtstbij
                    }
                }

                if (nearestSlot != null) // Als een slot gevonden is
                {
                    nearestSlot.gameObject.SetActive(true); // Zet slot aan
                    nearestSlot.GetComponent<Image>().sprite = _currentItem.GetComponent<Image>().sprite; // Zet item icoon in slot
                    nearestSlot.item = _currentItem; // Sla item op in slot
                    itemList[nearestSlot.Index] = _currentItem;
                    CheckForCreatedRecipes();
                }

                _currentItem = null; // Speler heeft geen item meer vast
            }
        }
    }

    void CheckForCreatedRecipes()
    {
        // Zet alle result slots uit en maak ze leeg
        foreach (Slot slot in slotsResults)
        {
            slot.gameObject.SetActive(false);
            slot.item = null;
        }

        // Maak een string van alle items in de crafting slots
        string currentRecipeString = "";

        foreach (Item item in itemList)
        {
            if (item != null)
            {
                currentRecipeString += item.name; // voeg item naam toe
            }
            else
            {
                currentRecipeString += "null"; // lege slot
            }
        }

        // Check of deze combinatie een recipe is
        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i] == currentRecipeString)
            {
                // Zet result slot aan
                slotsResults[0].gameObject.SetActive(true);

                // Zet sprite van result item
                slotsResults[0].GetComponent<Image>().sprite =
                    recipeResults[i].GetComponent<Image>().sprite;

                // Sla result item op
                slotsResults[0].item = recipeResults[i];
            }
        }
    }
    public void OnClickSlot(Slot slot)
    {
        slot.item = null;
        itemList[slot.Index] = null;
        slot.gameObject.SetActive(false);
    }
    public void SelectItem(Item item)
    {
        if(_currentItem == null)
        {
            _currentItem = item; // Zet het aangeklikte item als het huidige item dat je vast hebt
            _customCursor.gameObject.SetActive(true);// Zet de custom cursor zichtbaar (cursor met item icoon)
            _customCursor.sprite = _currentItem.GetComponent<Image>().sprite;// Zet het item icoon op de cursor
        }
    }
}
