using UnityEngine;
using Unity.Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;

    // In Unity 6, we target the OrbitalFollow component specifically
    [SerializeField] private CinemachineOrbitalFollow orbitalFollow;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (InputManager.Instance != null)
            InputManager.Instance.OnLook += HandleLook;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnLook -= HandleLook;
    }

    private void HandleLook(Vector2 lookInput)
    {
        if (orbitalFollow == null) return;

        // X Axis is the Horizontal Orbit (Degrees)
        orbitalFollow.HorizontalAxis.Value += lookInput.x * sensitivityX;

        // Y Axis is the Vertical Orbit (0 to 1 range)
        orbitalFollow.VerticalAxis.Value += lookInput.y * sensitivityY;
    }
}