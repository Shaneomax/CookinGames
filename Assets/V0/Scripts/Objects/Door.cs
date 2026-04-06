using UnityEngine;

public class Door : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float smoothSpeed = 5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        // Save the starting rotation as "Closed"
        closedRotation = transform.rotation;
        // Calculate the "Open" rotation based on the Y axis
        openRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + openAngle, transform.eulerAngles.z);
    }

    private void Update()
    {
        // Smoothly rotate toward the target state
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    public void Interact()
    {
        isOpen = !isOpen; // Toggle state
        Debug.Log("Door is now " + (isOpen ? "Open" : "Closed"));
    }
}