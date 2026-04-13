using UnityEngine;
using UnityEngine.InputSystem;

public class CarController2 : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("Wheel Meshes")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    [Header("Car Settings")]
    public float motorForce = 1500f;
    public float steerAngle = 30f;
    public float brakeForce = 4000f;

    [Header("Center of Mass")]
    public Transform centerOfMassPoint; // 👈 drag empty GameObject here

    private Rigidbody rb;

    private float moveInput;
    private float steerInput;
    private bool isBraking;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ✅ Apply custom center of mass
        if (centerOfMassPoint != null)
        {
            rb.centerOfMass = transform.InverseTransformPoint(centerOfMassPoint.position);
        }
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // MOVE
        if (keyboard.wKey.isPressed)
            moveInput = 1f;
        else if (keyboard.sKey.isPressed)
            moveInput = -1f;
        else
            moveInput = 0f;

        // STEER
        if (keyboard.aKey.isPressed)
            steerInput = -1f;
        else if (keyboard.dKey.isPressed)
            steerInput = 1f;
        else
            steerInput = 0f;

        // BRAKE
        isBraking = keyboard.spaceKey.isPressed;
    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        HandleBraking();
        UpdateWheels();
    }

    void HandleMotor()
    {
        rearLeft.motorTorque = moveInput * motorForce;
        rearRight.motorTorque = moveInput * motorForce;
    }

    void HandleSteering()
    {
        float steer = steerInput * steerAngle;
        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;
    }

    void HandleBraking()
    {
        float brake = isBraking ? brakeForce : 0f;

        frontLeft.brakeTorque = brake;
        frontRight.brakeTorque = brake;
        rearLeft.brakeTorque = brake;
        rearRight.brakeTorque = brake;
    }

    void UpdateWheels()
    {
        UpdateWheel(frontLeft, frontLeftMesh);
        UpdateWheel(frontRight, frontRightMesh);
        UpdateWheel(rearLeft, rearLeftMesh);
        UpdateWheel(rearRight, rearRightMesh);
    }

    void UpdateWheel(WheelCollider col, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);

        mesh.position = pos;
        mesh.rotation = rot;
    }
}