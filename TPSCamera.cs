using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float smoothSpeed = 0.125f;
    public LayerMask obstacleMask;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 direction = desiredPosition - target.position;

        // Obstacle avoidance
        if (Physics.Raycast(target.position, direction.normalized, out RaycastHit hit, offset.magnitude, obstacleMask))
        {
            desiredPosition = hit.point;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
