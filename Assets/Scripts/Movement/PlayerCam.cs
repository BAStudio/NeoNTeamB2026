using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] private Transform orientation;

    [Header("HeadBobbing")]
    private float initialBobSpeed;

    public float bobbingSpeed = 10f;
    public float bobbingAmount = 0.2f;
    public float horizontalBobbingAmount = 0.02f;
    public Vector3 midpoint;

    private float timer = 0f;
    private float verticalRandomOffset;
    private float horizontalRandomOffset;
    private Vector3 bobOffset;
    private bool isMoving;

    [Header("Player Rigidbody")]
    [SerializeField] private Rigidbody playerRigidbody;

    [Header("Tilt Settings")]
    [SerializeField] private float tiltAmount = 5f;
    [SerializeField] private float tiltSmoothSpeed = 8f;

    [Header("Sprint Tilt")]
    [SerializeField] private float sprintTiltMultiplier = 1.5f;

    [Header("FOV Settings")]
    [SerializeField] private float sprintFovIncrease = 20f;
    [SerializeField] private float fovSmoothSpeed = 8f;

    [Header("Mount Camera Kick")]
    [SerializeField] private float mountKickTilt = 8f;
    [SerializeField] private float mountKickUp = 0.12f;
    [SerializeField] private float mountDuration = 0.35f;

    private float mountTimer;
    private bool mounting;
    private float mountYOffset;

    private float xRotation;
    private float yRotation;

    private float currentTiltZ;
    private float currentTiltX;

    private Camera camera;
    private float defaultFov;
    private float targetFov;

    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camera = GetComponent<Camera>();
        defaultFov = camera.fieldOfView;
        initialBobSpeed = bobbingSpeed;

        midpoint = Vector3.zero;
        verticalRandomOffset = Random.Range(0f, Mathf.PI * 2f);
        horizontalRandomOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        Look();
        HandleMovementInput();
        HandleTilt();
        HandleHeadBobbing();
        HandleFov();
    }

    public void TriggerMountKick()
    {
        mounting = true;
        mountTimer = 0f;
    }

    private void Look()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    private void HandleMovementInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (playerRigidbody != null)
        {
            Vector3 horizontalVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);
            isMoving = horizontalVelocity.magnitude > 0.1f;
        }
        else
        {
            isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
        }
    }

    private void HandleTilt()
    {
        float tiltMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintTiltMultiplier : 1f;

        float targetTiltZ = -horizontalInput * tiltAmount * tiltMultiplier;
        float targetTiltX = verticalInput * tiltAmount * tiltMultiplier;

        currentTiltZ = Mathf.Lerp(currentTiltZ, targetTiltZ, Time.deltaTime * tiltSmoothSpeed);
        currentTiltX = Mathf.Lerp(currentTiltX, targetTiltX, Time.deltaTime * tiltSmoothSpeed);

        float mountTilt = 0f;

        if (mounting)
        {
            mountTimer += Time.deltaTime;

            float t = mountTimer / mountDuration;
            float smooth = Mathf.SmoothStep(0, 1, t);

            mountTilt = Mathf.Sin(smooth * Mathf.PI) * mountKickTilt;

            if (mountTimer >= mountDuration)
                mounting = false;
        }

        Quaternion lookRotation = Quaternion.Euler(xRotation, yRotation, 0);
        Quaternion tiltRotation = Quaternion.Euler(currentTiltX + mountTilt, 0f, currentTiltZ);

        transform.rotation = lookRotation * tiltRotation;

        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void HandleHeadBobbing()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            bobbingSpeed = initialBobSpeed * 2;
        else
            bobbingSpeed = initialBobSpeed;

        if (isMoving)
        {
            timer += bobbingSpeed * Time.deltaTime;

            if (timer > Mathf.PI * 2f)
                timer -= Mathf.PI * 2f;

            float waveSlice = Mathf.Sin(timer + verticalRandomOffset);
            float verticalOffset = waveSlice * bobbingAmount;
            float horizontalOffset = Mathf.Cos(timer + horizontalRandomOffset) * horizontalBobbingAmount;

            Vector3 swayPosition = new Vector3(
                midpoint.x + horizontalOffset,
                midpoint.y + verticalOffset,
                midpoint.z
            );

            bobOffset = Vector3.Lerp(bobOffset, swayPosition, Time.deltaTime * 5f);
        }
        else
        {
            bobOffset = Vector3.Lerp(bobOffset, midpoint, Time.deltaTime * 5f);
        }

        if (mounting)
        {
            float t = mountTimer / mountDuration;
            float smooth = Mathf.SmoothStep(0, 1, t);

            float targetHeight = Mathf.Sin(smooth * Mathf.PI) * mountKickUp;
            mountYOffset = Mathf.Lerp(mountYOffset, targetHeight, Time.deltaTime * 10f);

            bobOffset.y += mountYOffset;
        }
        else
        {
            mountYOffset = Mathf.Lerp(mountYOffset, 0, Time.deltaTime * 10f);
        }
    }

    private void HandleFov()
    {
        targetFov = Input.GetKey(KeyCode.LeftShift)
            ? defaultFov + sprintFovIncrease
            : defaultFov;

        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFov, Time.deltaTime * fovSmoothSpeed);
    }

    public Vector3 GetBobOffset() => bobOffset;
}