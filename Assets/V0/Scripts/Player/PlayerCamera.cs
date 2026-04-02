using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera normalCamera;
    [SerializeField] private CinemachineCamera zoomCamera;

    [Header("Player Settings")]
    [SerializeField] private Transform playerTransform; // Assign your Player in the Inspector
    [SerializeField] private float rotationSpeed = 15f; // Controls how fast the player turns

    private Vector2 lookInput;

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
        RotatePlayerWithCamera();
    }

    private void RotatePlayerWithCamera()
    {
        if (playerTransform != null)
        {
            float cameraYaw = Camera.main.transform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, cameraYaw, 0f);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        }
    }
}