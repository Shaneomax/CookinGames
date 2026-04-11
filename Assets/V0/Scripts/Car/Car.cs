using Unity.Cinemachine;
using UnityEngine;

public class Car : MonoBehaviour, Iinteractable // Added interface
{
    public bool isDriverDoorOpen = false;
    public bool isPassengerDoorOpen = false;

    [Header("Car References")]
    [SerializeField] private CarController carController;
    [SerializeField] private CinemachineCamera carCamera;

    private PlayerController playerInside;

    private void Start()
    {
        // Ensure the car is off and camera is hidden by default
        if (carController != null) carController.enabled = false;
        if (carCamera != null) carCamera.Priority = -1;
    }

    // Called by PlayerController when standing outside and pressing interact
    public void Interact()
    {
        EnterCar();
    }

    private void EnterCar()
    {
        // Find the player object in the scene
        playerInside = Object.FindFirstObjectByType<PlayerController>();

        if (playerInside != null)
        {
            // 1. Disable player script, enable car script (GameObjects stay active)
            playerInside.enabled = false;
            carController.enabled = true;

            // 2. Parent the player to the car so they don't get left behind when driving!
            playerInside.transform.SetParent(transform);

            // 3. Switch to the Car Camera
            carCamera.Priority = 3;

            // 4. Subscribe to the InputManager to listen for the Exit command
            InputManager.Instance.OnInteract += ExitCar;
        }
    }

    // Called by InputManager when the player is inside the car and presses interact
    private void ExitCar()
    {
        if (playerInside != null)
        {
            // 1. Unsubscribe immediately so this doesn't fire accidentally later
            InputManager.Instance.OnInteract -= ExitCar;

            // 2. Enable player script, disable car script
            playerInside.enabled = true;
            carController.enabled = false;

            // 3. Unparent the player so they stay exactly where they get out
            playerInside.transform.SetParent(null);

            // 4. Revert to the Player Camera
            carCamera.Priority = -1;

            // Clear the reference
            playerInside = null;
        }
    }
}