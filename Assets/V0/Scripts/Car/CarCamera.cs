using Unity.Cinemachine;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [Header("Camera Setup")]
    [SerializeField] private CinemachineCamera carCinemachineCamera;
    [SerializeField] private Transform carTransform;
    [SerializeField] private Transform pivotTransform;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minPitch = -10f; 
    [SerializeField] private float maxPitch = 60f;

    private Vector2 lookInput;
    private float yaw;
    private float pitch;

    private void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnLook += HandleLook;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnLook -= HandleLook;
        }
    }

    private void HandleLook(Vector2 input)
    {
        lookInput = input;
    }

    private void LateUpdate()
    {
        if (carTransform == null || pivotTransform == null) return;

        pivotTransform.position = carTransform.position;

        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        yaw = Mathf.Repeat(yaw, 360f);

        pivotTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}