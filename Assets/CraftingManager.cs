using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : Item
{
    private Item _currentItem;
   [SerializeField] private Image _customCursor;

  [SerializeField]  private Slot[] craftingSlots;

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
                }

                _currentItem = null; // Speler heeft geen item meer vast
            }
        }
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
