using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    public static bool IsOpen { get; private set; }
    [SerializeField] private Image inventoryBorder;
    [SerializeField] private GameObject[] slots;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.3f;

    private RectTransform borderRect;
    private Coroutine toggleCoroutine;

    void Start()
    {
        if (inventoryBorder != null)
        {
            borderRect = inventoryBorder.GetComponent<RectTransform>();
            // Start closed: scale width to 0
            borderRect.localScale = new Vector3(0f, borderRect.localScale.y, borderRect.localScale.z);
            inventoryBorder.enabled = false;
        }

        foreach (GameObject slot in slots)
        {
            if (slot != null) slot.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (IsOpen)
                CloseInventory();
            else
                OpenInventory();
        }
    }

    public void OpenInventory()
    {
        if (IsOpen) return;

        IsOpen = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (toggleCoroutine != null) StopCoroutine(toggleCoroutine);
        toggleCoroutine = StartCoroutine(OpenSequence());
    }

    public void CloseInventory()
    {
        if (!IsOpen) return;

        IsOpen = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (toggleCoroutine != null) StopCoroutine(toggleCoroutine);
        toggleCoroutine = StartCoroutine(CloseSequence());
    }

    private IEnumerator OpenSequence()
    {
        if (inventoryBorder != null) inventoryBorder.enabled = true;

        // Turn on slots so they can visually scale or display
        foreach (GameObject slot in slots)
        {
            if (slot != null) slot.SetActive(true);
        }

        // Run the book opening animation
        UIAnimations.BookTransition(this, borderRect, true, animationDuration);

        // Wait out the animation duration using unscaled time
        yield return new WaitForSecondsRealtime(animationDuration);
    }

    private IEnumerator CloseSequence()
    {
        // Run the book closing animation
        UIAnimations.BookTransition(this, borderRect, false, animationDuration);

        yield return new WaitForSecondsRealtime(animationDuration);

        // Turn off components cleanly AFTER animation completes
        foreach (GameObject slot in slots)
        {
            if (slot != null) slot.SetActive(false);
        }

        if (inventoryBorder != null) inventoryBorder.enabled = false;
    }
}