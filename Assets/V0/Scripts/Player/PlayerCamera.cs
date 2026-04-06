using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera normalCamera;
    [SerializeField] private CinemachineCamera zoomCamera;

    [Header("Player Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private float rotationSpeed = 15f;
    private CinemachineBrain brain;
    private Vector2 lookInput;

    private float yaw;
    private float pitch;
    [SerializeField] private float mouseSensitivity = 2f;

    private void Awake()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        InputManager.Instance.OnZoom += HandleZoom;
        InputManager.Instance.OnLook += HandleLook;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnZoom -= HandleZoom;
        InputManager.Instance.OnLook -= HandleLook;
    }

    private void HandleZoom(bool isZooming)
    {
        if (isZooming)
        {
            zoomCamera.Priority = 2;
            normalCamera.Priority = 1;
        }
        else
        {
            zoomCamera.Priority = 0;
            normalCamera.Priority = 1;
        }
    }

    private void HandleLook(Vector2 input)
    {
        lookInput = input;
    }


    private void LateUpdate()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        // 1. Get Mouse Input
        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;

        // 2. Clamp Pitch (Vertical look) - Keep this so the camera doesn't flip upside down
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        // 3. DO NOT CLAMP YAW (Horizontal look)
        // This keeps the value clean between 0-360 for infinite rotation
        yaw = Mathf.Repeat(yaw, 360f);

        // 4. Update the Camera Pivot (Free movement)
        pivotTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        if (zoomCamera.Priority == 2)
        {
            // 5. Clamp Gun X (Pitch) - The gun won't tilt as far as the camera
            float clampedGunPitch = Mathf.Clamp(pitch, -20f, 20f);

            // Apply rotation to the gun (World rotation)
            gunTransform.rotation = Quaternion.Euler(clampedGunPitch, yaw, 0f);

            // 6. Rotate player body on Y axis (Yaw) only
            Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
            playerTransform.rotation = Quaternion.Slerp(
                playerTransform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
        else
        {
            // Reset gun to local forward when not zooming
            gunTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}