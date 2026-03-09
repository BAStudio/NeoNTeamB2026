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

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        initialSpeed = moveSpeed;

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
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
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
            climbCheckTransform.forward, out hit, 1f))
        {
            if (hit.transform.CompareTag("ClimableObject"))
            {
                Vector3 targetPosition =
                    hit.transform.position + Vector3.up * playerHeight;

                playerCam.TriggerMountKick();
                StartCoroutine(SmoothClimb(targetPosition, 0.5f));
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