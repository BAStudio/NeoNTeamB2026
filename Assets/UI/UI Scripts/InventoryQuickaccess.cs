using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryQuickaccess : MonoBehaviour
{
    [SerializeField] private GameObject[] quickAccess = new GameObject[4];
    [SerializeField] private TextMeshProUGUI[] slotTexts = new TextMeshProUGUI[4];
    [SerializeField] private Image[] slotBackgrounds = new Image[4];
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private Color emptyColor = Color.white;

    int currentIndex = 0;

    private void Start()
    {
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.OnInventoryChanged += OnInventoryChanged;
            UpdateUI();
        }
    }

    private void OnEnable()
    {
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.OnInventoryChanged += OnInventoryChanged;
        }
    }

    private void OnDisable()
    {
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.OnInventoryChanged -= OnInventoryChanged;
        }
    }

    private void OnInventoryChanged()
    {
        for (int i = 0; i < 4; i++)
        {
            if (quickAccess[i] != null)
            {
                int mainIndex = FindItemInMainInventory(quickAccess[i]);
                if (mainIndex == -1)
                {
                    quickAccess[i] = null;
                }
            }
        }
        UpdateUI();
    }


    private void Update()
    {
        if (InventorySystem.Instance == null)
            return;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            int inventorySlot = InventorySystem.Instance.GetCurrentSlotIndex();
            GameObject item = InventorySystem.Instance.GetItemInSlot(inventorySlot);
            if (item != null)
            {
                quickAccess[currentIndex] = item;
                UpdateUI();
            }
        }
        if (InventoryToggle.IsOpen)
            return;

        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectQuickSlot(i);
                break;
            }
        }
    }

    private void SelectQuickSlot(int slotIndex)
    {
        currentIndex = slotIndex;
        GameObject item = quickAccess[currentIndex];

        if (item != null)
        {
            int mainInventoryIndex = FindItemInMainInventory(item);

            if (mainInventoryIndex != -1)
            {
                if (mainInventoryIndex != InventorySystem.Instance.CurrentSlot)
                {
                    InventorySystem.Instance.ChangeSlot(mainInventoryIndex);
                }
            }
        }
        else
        {
            int emptySlot = FindEmptyInventorySlot();
            if (emptySlot != -1)
            {
                InventorySystem.Instance.ChangeSlot(emptySlot);
            }
        }

        UpdateUI();
    }

    private int FindEmptyInventorySlot()
    {
        for (int i = 0; i < 12; i++)
        {
            if (InventorySystem.Instance.GetItemInSlot(i) == null)
                return i;
        }
        return -1;
    }

    public void ClearDroppedItems()
    {
        for (int i = 0; i < 4; i++)
        {
            if (quickAccess[i] != null)
            {
                int mainIndex = FindItemInMainInventory(quickAccess[i]);
                if (mainIndex == -1)
                {
                    quickAccess[i] = null;
                }
            }
        }
        UpdateUI();
    }

    private int FindItemInMainInventory(GameObject item)
    {
        for (int i = 0; i < 12; i++)
        {
            if (InventorySystem.Instance.GetItemInSlot(i) == item)
                return i;
        }
        return -1;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < 4; i++)
        {
            if (slotBackgrounds[i] != null)
            {
                slotBackgrounds[i].color = i == currentIndex ? activeColor 
                    : quickAccess[i] != null ? inactiveColor 
                    : emptyColor;
            }

            if (slotTexts[i] != null)
            {
                slotTexts[i].text = quickAccess[i] != null ? quickAccess[i].name : "";
                slotTexts[i].enabled = quickAccess[i] != null;
            }
        }
    }
}
