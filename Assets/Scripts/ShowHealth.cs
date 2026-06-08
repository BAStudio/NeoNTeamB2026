using UnityEngine;

public class ShowHealth : MonoBehaviour
{
    [SerializeField] private KeyCode PopupKey = KeyCode.T;
    [SerializeField] private GameObject HealthWatch;
    [SerializeField] private float PopupSpeed = 20f;
    [SerializeField] private float PopupAmount = 0.9f; // Keep this small for UI/Local space!

    private bool IsUP;
    private Vector3 startLocalPosition;
    private Vector3 targetLocalPosition;

    void Start()
    {
        if (HealthWatch != null)
        {
            // Save the LOCAL position relative to the camera
            startLocalPosition = HealthWatch.transform.localPosition;
        }
    }

    void Update()
    {
        if (HealthWatch == null) return;

        // 1. Toggle state on key press
        if (Input.GetKeyDown(PopupKey))
        {
            IsUP = !IsUP;
        }

        // 2. Calculate target in LOCAL space
        if (IsUP)
        {
            targetLocalPosition = startLocalPosition + new Vector3(0, PopupAmount, 0);
        }
        else
        {
            targetLocalPosition = startLocalPosition;
        }

        // 3. Smoothly move using localPosition
        HealthWatch.transform.localPosition = Vector3.Lerp(
            HealthWatch.transform.localPosition,
            targetLocalPosition,
            Time.deltaTime * PopupSpeed
        );
    }
}