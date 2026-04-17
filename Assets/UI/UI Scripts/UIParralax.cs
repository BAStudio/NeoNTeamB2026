using UnityEngine;

public class UILockedParallax : MonoBehaviour
{
    [SerializeField] float sensitivity = 5f;
    [SerializeField] float maxShift = 30f; // Limits how far it can drift

    Vector3 startPos;
    Vector3 currentOffset;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Capture relative movement (works while locked)
        float moveX = Input.GetAxis("Mouse X") * sensitivity;
        float moveY = Input.GetAxis("Mouse Y") * sensitivity;

        // Add to our current offset
        currentOffset.x += moveX;
        currentOffset.y += moveY;

        // Clamp the movement so the UI doesn't fly off the screen
        currentOffset.x = Mathf.Clamp(currentOffset.x, -maxShift, maxShift);
        currentOffset.y = Mathf.Clamp(currentOffset.y, -maxShift, maxShift);

        // Apply to the UI element
        transform.localPosition = startPos + currentOffset;

        // Optional: Slowly return to center when not moving
        currentOffset = Vector3.Lerp(currentOffset, Vector3.zero, Time.deltaTime * 2f);
    }
}