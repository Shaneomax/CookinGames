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
        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -30f, 60f);

        // ALWAYS rotate pivot (camera up/down + base rotation)
        pivotTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        

        // IF ZOOMING → rotate player ONLY on Y axis
        if (zoomCamera.Priority == 2)
        {
            gunTransform.rotation = pivotTransform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else 
        {
            gunTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

    }
}