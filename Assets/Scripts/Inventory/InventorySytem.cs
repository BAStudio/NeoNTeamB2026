using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance; // Hiermee kan je vanuit andere scripts makkelijk InventorySystem.Instance gebruiken

    public System.Action OnInventoryChanged;
    // Event dat wordt aangeroepen als inventory verandert

    public int CurrentSlot => currentSlot;
    // Geeft de huidige actieve slot terug

    public Transform handTransform;
    // Hier komt het actieve item in de hand van de speler

    private GameObject[] slots = new GameObject[4];
    // Je inventory heeft 4 slots: index 0, 1, 2 en 3

    public int currentSlot = 0;
    // Begint bij slot 1, maar in code is dat index 0

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Zet deze inventory als de enige Instance
        }
        else
        {
            Destroy(gameObject);
            // Voorkomt dubbele InventorySystems
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSlot(0);
        // Toets 1 selecteert slot 1

        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSlot(1);
        // Toets 2 selecteert slot 2

        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSlot(2);
        // Toets 3 selecteert slot 3

        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSlot(3);
        // Toets 4 selecteert slot 4

        if (Input.GetKeyDown(KeyCode.Q)) DropCurrentItem();
        // Met Q drop je het huidige item
    }

    public int GetCurrentSlotIndex()
    {
        return currentSlot;
        // Geeft de huidige slot index terug
    }

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // Zoekt naar een lege inventory slot

            if (slots[i] == null)
            {
                slots[i] = item;
                // Zet het item in de eerste lege slot

                if (i == currentSlot)
                {
                    item.SetActive(true);
                    // Als het item in de actieve slot komt, maak het zichtbaar

                    item.transform.SetParent(handTransform);
                    // Zet het item in de hand

                    item.transform.localPosition = Vector3.zero;
                    // Zet positie netjes op nul

                    item.transform.localRotation = Quaternion.identity;
                    // Reset rotatie
                }
                else
                {
                    item.SetActive(false);
                    // Als het niet de actieve slot is, verberg het item
                }

                OnInventoryChanged?.Invoke();
                // Laat andere scripts weten dat inventory veranderd is

                Debug.Log("Item added to slot " + (i + 1));
                // Laat zien in welke slot het item is toegevoegd

                PrintInventory();
                // Print de hele inventory in de Console

                return true;
                // Item is succesvol toegevoegd
            }
        }

        Debug.Log("Inventory full!");
        // Geen lege slot gevonden

        return false;
        // Item kon niet toegevoegd worden
    }

    public GameObject GetItemInSlot(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            return slots[index];
            // Geeft het item terug als de index geldig is
        }

        return null;
        // Geeft niks terug als de index ongeldig is
    }

    public void ChangeSlot(int newSlot)
    {
        if (newSlot < 0 || newSlot >= slots.Length)
        {
            Debug.LogWarning("Slot bestaat niet: " + newSlot);
            // Waarschuwing als je een slot probeert te kiezen die niet bestaat

            return;
        }

        if (newSlot == currentSlot) return;
        // Als je al op die slot zit, doe niks

        if (slots[currentSlot] != null)
        {
            slots[currentSlot].SetActive(false);
            // Verberg het oude item
        }

        currentSlot = newSlot;
        // Zet de nieuwe slot actief

        if (slots[currentSlot] != null)
        {
            GameObject item = slots[currentSlot];

            item.SetActive(true);
            // Maak het nieuwe item zichtbaar

            item.transform.SetParent(handTransform);
            // Zet het item in de hand

            item.transform.localPosition = Vector3.zero;
            // Reset positie

            item.transform.localRotation = Quaternion.identity;
            // Reset rotatie
        }

        OnInventoryChanged?.Invoke();
        // Laat weten dat inventory/actieve slot veranderd is

        Debug.Log("Switched to slot " + (currentSlot + 1));
        // Laat zien naar welke slot je bent gewisseld

        PrintInventory();
        // Print inventory in Console
    }

    public void DropCurrentItem()
    {
        if (slots[currentSlot] == null) return;
        // Als de huidige slot leeg is, doe niks

        GameObject item = slots[currentSlot];
        // Pakt het huidige item

        item.transform.SetParent(null);
        // Haalt het item uit de hand

        item.SetActive(true);
        // Maakt het item zichtbaar in de wereld

        Rigidbody rb = item.GetComponent<Rigidbody>();
        // Pakt de Rigidbody van het item

        if (rb != null)
        {
            rb.isKinematic = false;
            // Zet physics aan

            rb.detectCollisions = true;
            // Zet collision aan

            rb.linearVelocity = Vector3.zero;
            // Reset snelheid

            rb.angularVelocity = Vector3.zero;
            // Reset draaisnelheid

            rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);
            // Gooit het item vooruit
        }

        slots[currentSlot] = null;
        // Maakt de inventory slot leeg

        OnInventoryChanged?.Invoke();
        // Laat weten dat inventory veranderd is

        Debug.Log("Dropped: " + item.name);
        // Laat zien welk item is gedropt

        PrintInventory();
        // Print inventory
    }

    public bool HasItem(string idToFind)
    {
        foreach (GameObject itemObj in slots)
        {
            if (itemObj != null)
            {
                ItemPickup details = itemObj.GetComponent<ItemPickup>();
                // Pakt ItemPickup info

                if (details != null && details.itemID == idToFind)
                {
                    return true;
                    // Item gevonden
                }
            }
        }

        return false;
        // Item niet gevonden
    }

    void PrintInventory()
    {
        Debug.Log("=== INVENTORY ===");

        for (int i = 0; i < slots.Length; i++)
        {
            string itemName = slots[i] != null ? slots[i].name : "Empty";
            // Laat itemnaam zien of Empty als slot leeg is

            Debug.Log("Slot " + (i + 1) + ": " + itemName);
            // Print elke slot
        }
    }
}