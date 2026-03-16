using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public Transform handTransform;

    private GameObject[] slots = new GameObject[3];
    private int currentSlot = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSlot(2);

        if (Input.GetKeyDown(KeyCode.Q))
            DropCurrentItem();
    }

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;

                EquipIfCurrentSlot(i);

                return true;
            }
        }

        return false;
    }

    void EquipIfCurrentSlot(int slotIndex)
    {
        if (slotIndex == currentSlot)
        {
            GameObject item = slots[slotIndex];
            item.SetActive(true);
            item.transform.SetParent(handTransform);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }
        else
        {
            slots[slotIndex].SetActive(false);
        }
    }

    public void ChangeSlot(int newSlot)
    {
        if (newSlot == currentSlot) return;

        if (slots[currentSlot] != null)
            slots[currentSlot].SetActive(false);

        currentSlot = newSlot;

        if (slots[currentSlot] != null)
            EquipCurrentItem();
    }

    void EquipCurrentItem()
    {
        GameObject item = slots[currentSlot];

        item.SetActive(true);
        item.transform.SetParent(handTransform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    public void DropCurrentItem()
    {
        if (slots[currentSlot] == null) return;

        GameObject item = slots[currentSlot];

        item.transform.SetParent(null);
        item.SetActive(true);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);
        }

        slots[currentSlot] = null;
    }

    // Check of speler een Screwdriver heeft
    public bool HasScrewdriver()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null &&
                slots[i].GetComponent<Screwdriver>() != null)
            {
                return true;
            }
        }

        return false;
    }
}