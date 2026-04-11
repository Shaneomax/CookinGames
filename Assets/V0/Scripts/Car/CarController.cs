using Unity.Cinemachine;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Engine Settings")]
    public float motorForce = 3000f;
    public float maxSpeed = 70f;
    [Tooltip("How fast the car reaches full torque. Higher = Snappier")]
    public float acceleration = 5f;
    public float brakeForce = 3000f;
    public float maxSteerAngle = 25f;

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    [Header("Wheel Visuals")]
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    [SerializeField] private CinemachineCamera carCamera;
    private PlayerController currentPlayer;

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBrakeForce;
    private float activeTorque; // The actual torque being applied right now
    private bool isBraking;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += new Vector3(0, -0.5f, 0);
        rb.linearDamping = 0.5f;
        rb.angularDamping = 1.5f;
    }

    private void Start()
    {
        SetWheelFriction(frontLeftWheelCollider);
        SetWheelFriction(frontRightWheelCollider);
        SetWheelFriction(rearLeftWheelCollider);
        SetWheelFriction(rearRightWheelCollider);
    }

    void SetWheelFriction(WheelCollider wheel)
    {
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.stiffness = 2.5f; // Increase grip
        wheel.forwardFriction = forwardFriction;

        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = 3f; // VERY important for sliding
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        rb.AddForce(-transform.up * 100f);
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        float currentSpeed = rb.linearVelocity.magnitude;

        // 1. Calculate Target Torque based on input
        float targetTorque = verticalInput * motorForce;

        // 2. MANUALLY CONTROL ACCELERATION
        // This smoothly moves activeTorque towards targetTorque at the rate of 'acceleration'
        activeTorque = Mathf.MoveTowards(activeTorque, targetTorque, acceleration * motorForce * Time.fixedDeltaTime);

        // 3. Apply Speed Cap Falloff (so it doesn't jitter at max speed)
        float speedFactor = Mathf.InverseLerp(maxSpeed, maxSpeed * 0.9f, currentSpeed);
        float finalTorque = activeTorque * speedFactor;

        // Apply to AWD
        frontLeftWheelCollider.motorTorque = finalTorque;
        frontRightWheelCollider.motorTorque = finalTorque;
        rearLeftWheelCollider.motorTorque = finalTorque;
        rearRightWheelCollider.motorTorque = finalTorque;

        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    private void ApplyBraking()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        float speedFactor = Mathf.Clamp01(rb.linearVelocity.magnitude / maxSpeed);
        float dynamicSteer = Mathf.Lerp(maxSteerAngle, maxSteerAngle * 0.4f, speedFactor);

        currentSteerAngle = dynamicSteer * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }
}