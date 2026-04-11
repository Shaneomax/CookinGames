using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 moveInput;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float interactRange = 2f;

    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private GunController gunController;
    [SerializeField] private Collider[] colliderArray;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if(InputManager.Instance != null)
        {
            SubscribeInputs();
        }   

    }

    private void OnDisable()
    {
        if(InputManager.Instance != null)
        {
            InputManager.Instance.OnMove -= HandleMove;
            InputManager.Instance.OnJump -= HandleJump;
            InputManager.Instance.OnInteract -= HandleInteract;
            InputManager.Instance.OnFire -= HandleFire;
        }

    }

    private void SubscribeInputs()
    {
        // We unsubscribe first just to be 100% sure we don't accidentally double-subscribe, 
        // which would cause double-jumping or double-shooting.
        //InputManager.Instance.OnMove -= HandleMove;
        //InputManager.Instance.OnJump -= HandleJump;
        //InputManager.Instance.OnInteract -= HandleInteract;
        //InputManager.Instance.OnFire -= HandleFire;

        InputManager.Instance.OnMove += HandleMove;
        InputManager.Instance.OnJump += HandleJump;
        InputManager.Instance.OnInteract += HandleInteract;
        InputManager.Instance.OnFire += HandleFire;
    }

    private void HandleMove(Vector3 input)
    {
        moveInput = input;
    }

    private void HandleJump()
    {
        ApplyJumpForce();
    }

    private void HandleInteract()
    {
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out Iinteractable interactable))
            {
                interactable.Interact();
                return;
            }
        }
    }

    private void HandleFire()
    {
        gunController.Shoot();
    }

    private void FixedUpdate()
    {
       ApplyMovement();
    }

    public void ApplyMovement()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * moveInput.z + camRight * moveInput.x).normalized;

        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // Rotate Player towards movement direction when NOT zooming
        if (moveDirection != Vector3.zero && playerCamera.zoomCamera.Priority != 2)
        {
            Quaternion lookRot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * playerCamera.rotationSpeed);

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
        return Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
        Gizmos.DrawWireSphere(transform.position, interactRange);
        
    }


}
