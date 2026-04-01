using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput; // Changed to Vector2 for standard input
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rayDistance = 1.1f; // Adjusted for standard player height
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float upperLookLimit = 80f;
    [SerializeField] private float lowerLookLimit = -20f;
    [SerializeField] private float cameraDistance = 5f; // Distance from player

    private float xRotation = 0f; // Vertical (Pitch)
    private float yRotation = 0f; // Horizontal (Yaw)

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Lock cursor for better feel
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        InputManager.Instance.OnMove += HandleMove;
        InputManager.Instance.OnJump += HandleJump;
        InputManager.Instance.OnLook += HandleLook;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMove -= HandleMove;
        InputManager.Instance.OnJump -= HandleJump;
        InputManager.Instance.OnLook -= HandleLook;
    }

    private void HandleMove(Vector3 input) => moveInput = new Vector2(input.x, input.z);
    private void HandleJump() => ApplyJumpForce();

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    // LateUpdate is best for Cameras to ensure the player has moved first
    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void ApplyMovement()
    {
        // Movement is now relative to the Camera's forward direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDirection = (camForward.normalized * moveInput.y + camRight.normalized * moveInput.x) * moveSpeed;

        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);

        // Optional: Rotate player to face movement direction
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void HandleLook(Vector2 lookVector)
    {
        // Accumulate both rotations
        yRotation += lookVector.x * lookSpeed;
        xRotation -= lookVector.y * lookSpeed;

        // Clamp the vertical pitch
        xRotation = Mathf.Clamp(xRotation, lowerLookLimit, upperLookLimit);
    }

    private void UpdateCameraPosition()
    {
        // 1. Create rotation based on our Euler angles
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // 2. Calculate the position: Player Position - (Rotation * Forward Vector * Distance)
        Vector3 position = transform.position - (rotation * Vector3.forward * cameraDistance);

        // 3. Apply to camera
        cameraTransform.rotation = rotation;
        cameraTransform.position = position;
    }

    private void ApplyJumpForce()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayer);
    }
}