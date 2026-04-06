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
    [SerializeField] private float interactRange = 2f;

    public GunController gunController;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
       
    }

    private void Start()
    {
        InputManager.Instance.OnMove += HandleMove;
        InputManager.Instance.OnJump += HandleJump;
        InputManager.Instance.OnInteract += HandleInteract;
        InputManager.Instance.OnFire += HandleFire;

    }

    private void OnDisable()
    {
        InputManager.Instance.OnMove -= HandleMove;
        InputManager.Instance.OnJump -= HandleJump;
        InputManager.Instance.OnInteract -= HandleInteract;
        InputManager.Instance.OnFire -= HandleFire;
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
        
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPCInteractables npcInteractable))
            {
                npcInteractable.Interact();
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
        //Vector3 forward = Camera.main.transform.forward;
        //Vector3 right = Camera.main.transform.right;

        //forward.y = 0f;
        //right.y = 0f;
        //forward.Normalize();
        //right.Normalize();

        //Vector3 moveDirection = (forward * moveInput.z + right * moveInput.x).normalized;

        //rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);



        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.z).normalized;
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
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
        Gizmos.DrawWireSphere(transform.position, interactRange);
        
    }


}
