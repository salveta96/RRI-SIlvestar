using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BoatMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _moveActionRef;

    private Rigidbody _rb;
    private Vector2 _moveInput;

    public float speed = 1f;
    public float torque = 10f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _moveInput = _moveActionRef.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(transform.forward * speed * Mathf.Clamp01(_moveInput.y), ForceMode.Acceleration);
        _rb.AddTorque(transform.up * torque *_moveInput.x, ForceMode.Acceleration);
    }

    private void OnEnable()
    {
        _moveActionRef.action.Enable();
    }

    private void OnDisable()
    {
        _moveActionRef.action.Disable();
    }
}
