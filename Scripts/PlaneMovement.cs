using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;

public class PlaneMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 _rotationInput;
    private float _thrustInput;

    private float _gravityModifier; // 0 - 1 (full gravity - no gravity)
    private float _maxGravitiVelocity = 5f;
    private float _minGravityVelocity = 25f;

    public float _enginePower;
    public float _maxEnginePower;


    [SerializeField]
    public InputActionReference inputRotation;
    public InputActionReference inputThrust;
    public float torque;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _rotationInput = inputRotation.action.ReadValue<Vector3>();
        _thrustInput = inputThrust.action.ReadValue<float>();
        
        _gravityModifier = math.remap(_maxGravitiVelocity, _minGravityVelocity, 0f, 1f, math.clamp(rb.linearVelocity.magnitude, _maxGravitiVelocity, _minGravityVelocity));

        _enginePower = math.clamp(_enginePower + _thrustInput * 500 * Time.deltaTime, 0f, _maxEnginePower);

        Debug.Log($"Velocity: {rb.linearVelocity.magnitude}\t\tEnginePower: {_enginePower}\t\tGravityModifier:{_gravityModifier}");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity += (Vector3.up * 9.81f * Time.fixedDeltaTime) * _gravityModifier;

        rb.AddForce(-transform.forward * _enginePower);

        rb.AddTorque(-transform.up * torque * _rotationInput.z);
        rb.AddTorque(transform.forward * torque * _rotationInput.x);
        rb.AddTorque(-transform.right * torque * _rotationInput.y);
    }

    private void OnEnable()
    {
        inputRotation.action.Enable();
        inputThrust.action.Enable();
    }

    private void OnDisable()
    {
        inputRotation.action.Disable();
        inputThrust.action.Disable();
    }
}
