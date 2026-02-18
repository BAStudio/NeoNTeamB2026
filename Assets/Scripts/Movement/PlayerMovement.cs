using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float jumpHeight = 8f;
    [SerializeField] private float gravity = 20f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 100f;

    private Vector3 velocity;
    private float xRotation;
    private bool isRunning;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (characterController == null)
            characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
    }

    void LateUpdate()
    {
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * inputX + transform.forward * inputZ;

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runningSpeed : walkingSpeed;
        move *= speed;

        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
            velocity.y = jumpHeight;

        velocity.y -= gravity * Time.deltaTime;

        characterController.Move((move + velocity) * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
