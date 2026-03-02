using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] private Transform orientation;

    [Header("Tilt Settings")]
    [SerializeField] private float tiltAmount = 5f;
    [SerializeField] private float tiltSmoothSpeed = 8f;

    [Header("Sprint Tilt")]
    [SerializeField] private float sprintTiltMultiplier = 1.5f;

    [Header("FOV Settings")]
    [SerializeField] private float sprintFovIncrease = 20f;
    [SerializeField] private float fovSmoothSpeed = 8f;

    private float xRotation;
    private float yRotation;

    private float currentTiltZ;
    private float currentTiltX;

    private Camera camera;
    private float defaultFov;
    private float targetFov;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camera = GetComponent<Camera>();
        defaultFov = camera.fieldOfView;
    }

    private void Update()
    {
        Look();
        HandleTilt();
        HandleFov();
    }

    private void Look()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    private void HandleTilt()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float tiltMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintTiltMultiplier : 1f;

        float targetTiltZ = -horizontalInput * tiltAmount * tiltMultiplier;
        float targetTiltX = verticalInput * tiltAmount * tiltMultiplier;

        currentTiltZ = Mathf.Lerp(currentTiltZ, targetTiltZ, Time.deltaTime * tiltSmoothSpeed);
        currentTiltX = Mathf.Lerp(currentTiltX, targetTiltX, Time.deltaTime * tiltSmoothSpeed);

        Quaternion lookRotation = Quaternion.Euler(xRotation, yRotation, 0);
        Quaternion tiltRotation = Quaternion.Euler(currentTiltX, 0f, currentTiltZ);

        transform.rotation = lookRotation * tiltRotation;

        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void HandleFov()
    {
        targetFov = Input.GetKey(KeyCode.LeftShift)
            ? defaultFov + sprintFovIncrease
            : defaultFov;

        camera.fieldOfView = Mathf.Lerp(
            camera.fieldOfView,
            targetFov,
            Time.deltaTime * fovSmoothSpeed
        );
    }
}