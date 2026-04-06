using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera normalCamera;
    public CinemachineCamera zoomCamera;

    [Header("Player Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private Transform gunTransform;
    public float rotationSpeed = 15f;
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
        // Get Mouse Input
        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;

        // Clamp Vertical Look
        // Setting this to -30 to 60 is standard for a good range of motion
        pitch = Mathf.Clamp(pitch, -30f, 60f);
        yaw = Mathf.Repeat(yaw, 360f);

        //Update the Camera Pivot
        pivotTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        if (zoomCamera.Priority == 2)
        {
            // Rotate the WHOLE PLAYER on both Pitch (X) and Yaw (Y)
            // This makes the player character lean up and down with the mouse
            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);

            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        }
        
    }
}