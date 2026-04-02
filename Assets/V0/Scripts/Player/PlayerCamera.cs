using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomLerpSpeed = 10f;
    [SerializeField] private float zoomedDistance = 5f;
    [SerializeField] private float normalDistance = 10f;

    [SerializeField] private CinemachineCamera normalCamera;
    [SerializeField] private CinemachineCamera zoomCamera;

    private void Start()
    {

        InputManager.Instance.OnZoom += HandleZoom;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnZoom -= HandleZoom;
    }

    private void HandleZoom(bool isZooming)
    {
        if (isZooming)
        {
            zoomCamera.Priority = 2;     // Higher than normal
            normalCamera.Priority = 1;
        }
        else
        {
            zoomCamera.Priority = 0;
            normalCamera.Priority = 1;
        }
    }

    


}