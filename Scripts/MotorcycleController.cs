using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    InputActionReference _moveActionRef;
    [SerializeField]
    InputActionReference _brakeActionRef;

    private Vector2 _moveInput;
    private float _brakeInput;
    private float _currentAngle;

    public Rigidbody sphereRb, bodyRb;

    public float maxSpeed, acceleration, steerStrenght, tiltAngle, tiltSpeed;
    void Start()
    {
        sphereRb.transform.parent = null;
        bodyRb.transform.parent = null;
        _currentAngle = 0;
    }

    void Update()
    {
        _moveInput = _moveActionRef.action.ReadValue<Vector2>();
        _brakeInput = _brakeActionRef.action.ReadValue<float>();

        transform.position = sphereRb.transform.position;

        float targetAngle = tiltAngle * -_moveInput.x;

        if(sphereRb.linearVelocity.magnitude < maxSpeed / 10)
        {
            targetAngle = 0;
        }

        _currentAngle = math.clamp(_currentAngle + (targetAngle - _currentAngle) * Time.deltaTime * tiltSpeed, -tiltAngle, tiltAngle);

        bodyRb.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            -_currentAngle));

        Debug.Log(_brakeInput);
    }

    private void FixedUpdate()
    {
        float yVel = sphereRb.linearVelocity.y;
        sphereRb.linearVelocity = Vector3.Lerp(sphereRb.linearVelocity, (_brakeInput > 0 ? maxSpeed / 10 : maxSpeed) * _moveInput.y * -transform.forward, acceleration * Time.fixedDeltaTime / (_brakeInput > 0 ? 30 : 10));
        sphereRb.linearVelocity = new Vector3(sphereRb.linearVelocity.x, yVel, sphereRb.linearVelocity.z);

        transform.Rotate(transform.up, _moveInput.x * steerStrenght * (_brakeInput > 0 ? 2 : 1) * math.clamp(-transform.InverseTransformDirection(bodyRb.linearVelocity).z / 5, -1, 1) * Time.fixedDeltaTime);
    }
}
