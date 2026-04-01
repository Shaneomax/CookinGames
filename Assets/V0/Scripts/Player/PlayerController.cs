using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 moveInput;
    [SerializeField] private float moveSpeed = 7f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputManager.Instance.OnMove += HandleMove;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMove -= HandleMove;
    }

    private void HandleMove(Vector3 input)
    {
        moveInput = input;
    }

    private void FixedUpdate()
    {
       ApplyMovement();
    }

    public void ApplyMovement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, moveInput.z * moveSpeed);
        rb.linearVelocity = moveDirection;
    }

    
}
