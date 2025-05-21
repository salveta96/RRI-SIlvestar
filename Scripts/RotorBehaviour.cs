using UnityEngine;

public class RotorBehaviour : MonoBehaviour
{
    [SerializeField]
    private PlaneMovement _planeMovement;

    private float _rotationSpeed;
    void Start()
    {
        _rotationSpeed = 0;
    }

    
    void Update()
    {
        _rotationSpeed = _planeMovement._enginePower;
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }
}
