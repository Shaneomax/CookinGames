using UnityEngine;

public class SlidingDoor : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float slideDistance = 2f;
    [SerializeField] private float smoothSpeed = 5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        closedPosition = transform.localPosition;
        // Adjust Vector3.right if you want it to slide on a different axis
        openPosition = closedPosition + (Vector3.right * slideDistance);
    }

    private void Update()
    {
        Vector3 targetPos = isOpen ? openPosition : closedPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothSpeed);
    }

    public void Interact()
    {
        isOpen = !isOpen;
        Debug.Log("Sliding Door is " + (isOpen ? "Open" : "Closed"));
    }
}