using Unity.Cinemachine;
using UnityEngine;

public class Car : MonoBehaviour, Iinteractable // Added interface
{
    public bool isDriverDoorOpen = false;
    public bool isPassengerDoorOpen = false;

    [Header("Car References")]
    [SerializeField] private CarController carController;
    [SerializeField] private CinemachineCamera carCamera;

    [SerializeField]private PlayerController playerInside;

    private void Start()
    {
        if (carController != null) carController.enabled = false;
        if (carCamera != null) carCamera.Priority = -1;
    }

    public void Interact()
    {
        EnterCar();
    }

    private void EnterCar()
    {
       playerInside = FindFirstObjectByType<PlayerController>();

        if (playerInside != null)
        {
            playerInside.enabled = false;
            carController.enabled = true;

            playerInside.transform.SetParent(transform);

            carCamera.Priority = 3;

            InputManager.Instance.OnInteract += ExitCar;
        }
    }

    private void ExitCar()
    {
        if (playerInside != null)
        {
            InputManager.Instance.OnInteract -= ExitCar;

            playerInside.enabled = true;
            carController.enabled = false;

            playerInside.transform.SetParent(null);

            carCamera.Priority = -1;
            playerInside = null;
        }
    }
}