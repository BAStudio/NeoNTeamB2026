using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    // Hiermee kunnen andere scripts jouw inventory vinden via InventorySystem.Instance

    public System.Action OnInventoryChanged;
    // Wordt aangeroepen als er iets in de inventory verandert

    public int CurrentSlot => currentSlot;
    // Geeft de actieve hotbar slot terug

    public Transform handTransform;
    // Hier sleep je in Unity je hand/hold transform van je Player in

    private GameObject[] slots = new GameObject[17];
    // 17 totale plekken:
    // index 0 t/m 4 = Hotbar
    // index 5 t/m 16 = Inventory

    public int currentSlot = 0;
    // Je actieve hotbar slot begint bij slot 1, maar in code is dat index 0

    private const int hotbarSlotCount = 5;
    // Je hebt 5 hotbar slots

    private const int inventorySlotCount = 12;
    // Je hebt 12 inventory slots

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Zet dit object als centrale InventorySystem
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
        // Toets 1 selecteert hotbar slot 1

        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSlot(1);
        // Toets 2 selecteert hotbar slot 2

        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSlot(2);
        // Toets 3 selecteert hotbar slot 3

        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSlot(3);
        // Toets 4 selecteert hotbar slot 4

        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeSlot(4);
        // Toets 5 selecteert hotbar slot 5

        if (Input.GetKeyDown(KeyCode.Q)) DropCurrentItem();
        // Met Q drop je het item uit je actieve hotbar slot
    }

    public int GetCurrentSlotIndex()
    {
        return currentSlot;
        // Geeft de actieve hotbar slot index terug
    }

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // Zoekt de eerste lege plek in hotbar + inventory

            if (slots[i] == null)
            {
                return AddItemToSlot(i, item);
                // Voegt item toe aan eerste lege plek
            }
        }

        Debug.Log("Inventory is vol!");
        // Geen lege plek gevonden

        return false;
        // Item kon niet worden toegevoegd
    }

    public bool AddItemToHotbarSlot(int hotbarSlotIndex, GameObject item)
    {
        if (hotbarSlotIndex < 0 || hotbarSlotIndex >= hotbarSlotCount)
        {
            Debug.LogWarning("Hotbar slot bestaat niet: " + hotbarSlotIndex);
            return false;
            // Stop als hotbar index ongeldig is
        }

        return AddItemToSlot(hotbarSlotIndex, item);
        // Hotbar gebruikt index 0 t/m 4
    }

    public bool AddItemToInventorySlot(int inventorySlotIndex, GameObject item)
    {
        if (inventorySlotIndex < 0 || inventorySlotIndex >= inventorySlotCount)
        {
            Debug.LogWarning("Inventory slot bestaat niet: " + inventorySlotIndex);
            return false;
            // Stop als inventory index ongeldig is
        }

        int realSlotIndex = inventorySlotIndex + hotbarSlotCount;
        // InventorySlot 1 heeft slotIndex 0, maar wordt echte index 5
        // InventorySlot 2 wordt index 6
        // InventorySlot 12 wordt index 16
        // Daardoor komt het item NIET in de hotbar

        return AddItemToSlot(realSlotIndex, item);
        // Voegt item toe aan echte inventory plek
    }

    public bool AddItemToSlot(int slotIndex, GameObject item)
    {
        if (item == null)
        {
            Debug.LogWarning("Item is null.");
            return false;
            // Stop als er geen item is meegegeven
        }

        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogWarning("Slot bestaat niet: " + slotIndex);
            return false;
            // Stop als de slot index buiten de array valt
        }

        if (slots[slotIndex] != null)
        {
            Debug.Log("Slot " + (slotIndex + 1) + " is al bezet.");
            return false;
            // Stop als er al een item in deze slot zit
        }

        slots[slotIndex] = item;
        // Slaat het item echt op in de inventory-array

        if (slotIndex == currentSlot)
        {
            item.SetActive(true);
            // Alleen zichtbaar als het item in je actieve hotbar slot zit

            if (handTransform != null)
            {
                item.transform.SetParent(handTransform);
                // Zet het item in de hand van de speler

                item.transform.localPosition = Vector3.zero;
                // Reset positie

                item.transform.localRotation = Quaternion.identity;
                // Reset rotatie
            }
        }
        else
        {
            item.SetActive(false);
            // Items buiten de actieve hotbar slot worden verborgen
        }

        OnInventoryChanged?.Invoke();
        // Laat andere scripts weten dat de inventory veranderd is

        Debug.Log("Item toegevoegd aan echte slot index: " + slotIndex);
        // Laat zien in welke echte index hij zit

        PrintInventory();
        // Print overzicht in Console

        return true;
        // Item is toegevoegd
    }

    public GameObject GetItemInSlot(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            return slots[index];
            // Geeft item terug als index klopt
        }

        return null;
        // Geeft niks terug als index ongeldig is
    }

    public void ChangeSlot(int newSlot)
    {
        if (newSlot < 0 || newSlot >= hotbarSlotCount)
        {
            Debug.LogWarning("Hotbar slot bestaat niet: " + newSlot);
            return;
            // Je mag alleen wisselen tussen hotbar slot 0 t/m 4
        }

        if (newSlot == currentSlot) return;
        // Als je al op deze slot zit, doe niks

        if (slots[currentSlot] != null)
        {
            slots[currentSlot].SetActive(false);
            // Verberg item uit oude hotbar slot
        }

        currentSlot = newSlot;
        // Zet nieuwe hotbar slot actief

        if (slots[currentSlot] != null)
        {
            GameObject item = slots[currentSlot];
            // Pakt item uit nieuwe hotbar slot

            item.SetActive(true);
            // Maakt item zichtbaar

            if (handTransform != null)
            {
                item.transform.SetParent(handTransform);
                // Zet item in de hand

                item.transform.localPosition = Vector3.zero;
                // Reset positie

                item.transform.localRotation = Quaternion.identity;
                // Reset rotatie
            }
        }

        OnInventoryChanged?.Invoke();
        // Laat weten dat actieve slot veranderd is

        Debug.Log("Gewisseld naar hotbar slot " + (currentSlot + 1));
        // Console melding

        PrintInventory();
        // Print inventory overzicht
    }

    public void DropCurrentItem()
    {
        if (slots[currentSlot] == null)
        {
            Debug.Log("Geen item om te droppen.");
            return;
            // Stop als actieve hotbar slot leeg is
        }

        GameObject item = slots[currentSlot];
        // Pakt item uit actieve hotbar slot

        item.transform.SetParent(null);
        // Haalt item uit de hand

        item.SetActive(true);
        // Maakt item zichtbaar in de wereld

        Rigidbody rb = item.GetComponent<Rigidbody>();
        // Pakt Rigidbody als die bestaat

        if (rb != null)
        {
            rb.isKinematic = false;
            // Zet physics aan

            rb.detectCollisions = true;
            // Zet collisions aan

            rb.linearVelocity = Vector3.zero;
            // Reset snelheid

            rb.angularVelocity = Vector3.zero;
            // Reset draaiing

            if (Camera.main != null)
            {
                rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);
                // Gooit item naar voren
            }
        }

        slots[currentSlot] = null;
        // Maakt actieve hotbar slot leeg

        OnInventoryChanged?.Invoke();
        // Laat weten dat inventory veranderd is

        Debug.Log("Gedropt: " + item.name);
        // Console melding

        PrintInventory();
        // Print inventory overzicht
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
        Debug.Log("=== HOTBAR ===");

        for (int i = 0; i < hotbarSlotCount; i++)
        {
            string itemName = slots[i] != null ? slots[i].name : "Empty";
            Debug.Log("Hotbar Slot " + (i + 1) + ": " + itemName);
            // Print hotbar slots 1 t/m 5
        }

        Debug.Log("=== INVENTORY ===");

        for (int i = hotbarSlotCount; i < slots.Length; i++)
        {
            string itemName = slots[i] != null ? slots[i].name : "Empty";
            Debug.Log("Inventory Slot " + (i - hotbarSlotCount + 1) + ": " + itemName);
            // Print inventory slots 1 t/m 12
        }
    }
}