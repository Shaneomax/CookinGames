using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //[SerializeField] private float zoomSpeed = 2f;
    //[SerializeField] private float zoomLerpSpeed = 10f;
    //[SerializeField] private float zoomedDistance = 5f;
    //[SerializeField] private float normalDistance = 10f;
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera normalCamera;
    [SerializeField] private CinemachineCamera zoomCamera;

    private Vector2 lookInput;
    private void Start()
    {

        InputManager.Instance.OnZoom += HandleZoom;
        //InputManager.Instance.OnLook += HandleLook;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnZoom -= HandleZoom;
        //InputManager.Instance.OnLook -= HandleLook;
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

    //private void HandleLook(Vector2 input)
    //{
    //    lookInput = input;
    //}

    //private void LateUpdate()
    //{
    //    RotateCamera();
    //}

    //private void RotateCamera()
    //{
       
    //}




}