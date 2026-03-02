using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    private float initialSpeed;


    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoolDown;
    [SerializeField] private float airMultiplier;
    [SerializeField] bool readyToJump = true;

    [SerializeField] private float groundDrag;

    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool grounded;

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform climbCheckTransform;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        initialSpeed = moveSpeed;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckMount();
        }

        if (Input.GetKey(KeyCode.LeftShift) && grounded)
        {
            moveSpeed = initialSpeed * sprintMultiplier;
        }
        else
        {
            moveSpeed = initialSpeed;
        }

    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
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
        if (Physics.Raycast(climbCheckTransform.position, climbCheckTransform.forward, out hit, 1f))
        {

            if (hit.transform.CompareTag("ClimableObject"))
            {
                Vector3 targetPosition = hit.transform.position + Vector3.up * playerHeight;
                StartCoroutine(SmoothClimb(targetPosition, 0.5f)); // 0.5f is the duration in seconds
                Debug.Log(hit.transform.name);
            }

            // Add this coroutine to your class:
            System.Collections.IEnumerator SmoothClimb(Vector3 targetPosition, float duration)
            {
                Vector3 startPosition = transform.position;
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;
            }
        }

    }
}