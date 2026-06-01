using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Needed for Text or Image components
using TMPro;           // Use this if you are using TextMeshPro

public class UISlot : MonoBehaviour
{
    [Header("Slot Configuration")]
    [SerializeField] Image backgroundVisual;
    [SerializeField] private int slotIndex; // Set this to 0, 1, 2, etc. in the Inspector
    [SerializeField] private TextMeshProUGUI nameText; // Drag your text component here

    [Header("Slot Colors")]
    [SerializeField] Color activeColor = Color.green; // Color when active
    [SerializeField] Color inactiveColor = Color.gray; // Color when inactive

    private void OnEnable()
    {
        // Try to subscribe; if InventorySystem isn't available yet, WaitForInventorySystem will subscribe later.
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.OnInventoryChanged += UpdateSlotVisuals;
            UpdateSlotVisuals();
        }
        else
        {
            StartCoroutine(WaitForInventorySystem());
        }
    }

    private void OnDisable()
    {
        // Only unsubscribe if the instance still exists
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.OnInventoryChanged -= UpdateSlotVisuals;
        }

        // stop any running coroutine to avoid double subscribes when re-enabled quickly
        StopCoroutine(WaitForInventorySystem());
    }

    private IEnumerator WaitForInventorySystem()
    {
        // Wait until InventorySystem.Instance exists (safeguard startup ordering)
        while (InventorySystem.Instance == null)
            yield return null;

        InventorySystem.Instance.OnInventoryChanged += UpdateSlotVisuals;
        UpdateSlotVisuals();
    }

    private void UpdateSlotVisuals()
    {
        if (InventorySystem.Instance == null)
            return; // nothing to show yet

        // 1. Get the item data
        GameObject item = InventorySystem.Instance.GetItemInSlot(slotIndex);

        // 2. Update the name text (guard null)
        if (nameText != null)
        {
            if (item != null)
            {
                nameText.text = item.name;
                nameText.enabled = true;
            }
            else
            {
                nameText.text = "";
                nameText.enabled = false;
            }
        }

        // 3. Handle the selection color logic (guard backgroundVisual)
        if (backgroundVisual != null)
        {
            int activeIndex = InventorySystem.Instance.GetCurrentSlotIndex();

            if (slotIndex == activeIndex)
            {
                backgroundVisual.color = activeColor;
            }
            else
            {
                backgroundVisual.color = inactiveColor;
            }
        }
    }
}