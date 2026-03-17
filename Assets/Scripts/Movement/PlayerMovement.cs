using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    private float initialSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoolDown;
    [SerializeField] private float airMultiplier;
    [SerializeField] bool readyToJump = true;

    [Header("Crouch")]
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float crouchSmoothSpeed = 8f;

    private float originalHeight;
    private float targetHeight;
    private bool isCrouching;

    [SerializeField] private Transform cameraHolder;
    private Vector3 originalCameraPos;
    private Vector3 crouchCameraPos;

    [Header("Physics")]
    [SerializeField] private float groundDrag;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool grounded;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform climbCheckTransform;
    [SerializeField] private PlayerCam playerCam;

    [Header("Mount Settings")]
    [SerializeField] private float mountDistance = 2f;
    [SerializeField] private float mountHeight = 1f;
    [SerializeField] private float mountDuration = 0.5f;

    [Header("Sounds")]
    [SerializeField] private AudioSource footstepAudioSource;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    [SerializeField] private float moveSpeedMultiplier = 1f;

    public void ApplySpeedMultiplier(float multiplier)
    {
        moveSpeedMultiplier *= Mathf.Clamp(multiplier, 0.1f, 2f);
    }

    public void ResetSpeed()
    {
        moveSpeedMultiplier = 1f;
    }
    // New: Expose if the player is moving based on Rigidbody velocity
    public bool IsMoving { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        initialSpeed = moveSpeed;
        moveSpeedMultiplier = 1f;

        originalHeight = transform.localScale.y;
        targetHeight = originalHeight;

        originalCameraPos = cameraHolder.localPosition;
        crouchCameraPos = new Vector3(
            originalCameraPos.x,
            originalCameraPos.y - 0.6f,
            originalCameraPos.z
        );
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down,
            playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        HandleCrouch();

        // Detect if moving based on Rigidbody velocity (XZ plane)
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        IsMoving = flatVel.magnitude > 0.1f; // 0.1f is a small threshold to ignore jitter

        if (IsMoving)
        {
            //Debug.Log("He hears me!");
            footstepAudioSource.Play();
        }

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded && !isCrouching)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            CheckMount();

        if (Input.GetKey(KeyCode.LeftShift) && grounded && !isCrouching)
            moveSpeed = initialSpeed * sprintMultiplier;
        else if (!isCrouching)
            moveSpeed = initialSpeed;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();

        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    private void HandleCrouch()
    {
        float currentHeight = transform.localScale.y;
        float newHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * crouchSmoothSpeed);

        transform.localScale = new Vector3(
            transform.localScale.x,
            newHeight,
            transform.localScale.z
        );

        Vector3 targetCamPos = isCrouching ? crouchCameraPos : originalCameraPos;

        cameraHolder.localPosition = Vector3.Lerp(
            cameraHolder.localPosition,
            targetCamPos,
            Time.deltaTime * crouchSmoothSpeed
        );
    }

    private void StartCrouch()
    {
        isCrouching = true;
        targetHeight = crouchHeight;
        moveSpeed = initialSpeed * crouchSpeedMultiplier;
    }

    private void StopCrouch()
    {
        isCrouching = false;
        targetHeight = originalHeight;
        moveSpeed = initialSpeed;
    }

    private void MovePlayer()
    {
        float effectiveSpeed = moveSpeed * moveSpeedMultiplier;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * effectiveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * effectiveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        float effectiveSpeed = moveSpeed * moveSpeedMultiplier;
        if (flatVel.magnitude > effectiveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * effectiveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void CheckMount()
    {
        RaycastHit hit;

        if (Physics.Raycast(climbCheckTransform.position,
            climbCheckTransform.forward, out hit, mountDistance))
        {
            if (hit.transform.CompareTag("ClimableObject"))
            {
                Vector3 forwardMove = orientation.forward * mountDistance;
                Vector3 upwardMove = Vector3.up * mountHeight;

                Vector3 targetPosition = transform.position + forwardMove + upwardMove;

                playerCam.TriggerMountKick();
                StartCoroutine(SmoothClimb(targetPosition, mountDuration));
            }
        }
    }

    System.Collections.IEnumerator SmoothClimb(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position =
                Vector3.Lerp(startPosition, targetPosition, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
