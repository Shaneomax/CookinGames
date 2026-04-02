using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 moveInput;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rayDistance = 5f;
    //[SerializeField] private bool isGrounded = true;
    [SerializeField] private float jumpForce = 5f;
    private bool checkGroundLayer = true;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InputManager.Instance.OnMove += HandleMove;
        InputManager.Instance.OnJump += HandleJump;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMove -= HandleMove;
        InputManager.Instance.OnJump -= HandleJump;
    }

    private void HandleMove(Vector3 input)
    {
        moveInput = input;
    }

    private void HandleJump()
    {
        ApplyJumpForce();
    }

    private void FixedUpdate()
    {
       ApplyMovement();
    }

    private void ApplyMovement()
    {
        // 1. Get Camera direction, but ignore Up/Down tilt
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // 2. Create the move direction relative to the camera
        Vector3 relativeDirection = (camForward * moveInput.z) + (camRight * moveInput.x);

        // 3. Apply to Rigidbody (keeping the existing Y velocity for gravity/jumping)
        rb.linearVelocity = new Vector3(
            relativeDirection.x * moveSpeed,
            rb.linearVelocity.y,
            relativeDirection.z * moveSpeed
        );

        // 4. Rotate player to face the direction they are walking
        if (relativeDirection.magnitude > 0.1f)
        {
            transform.forward = relativeDirection;
        }
    }

    private void ApplyJumpForce()
    {
        if (isGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool isGrounded()
    {
        checkGroundLayer = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayer);
        return checkGroundLayer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
    }


}
