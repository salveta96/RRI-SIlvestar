using UnityEngine;
using TMPro;
using System.Text;

public class HUD : MonoBehaviour
{
    public Rigidbody playerRb;                 // Assign in Inspector
    public TextMeshProUGUI speedText;          // Assign in Inspector
    public TextMeshProUGUI stateText;          // Assign in Inspector
    public PlayerController controllerScript; // Your movement script reference

    private StringBuilder stringBuilder = new StringBuilder();

    void Update()
    {
        if (playerRb == null || speedText == null || stateText == null || controllerScript == null)
        {
            Debug.LogWarning("HUD: Missing references. Please assign all required components.");
            return;
        }

        UpdateSpeed();
        UpdateState();
    }

    private void UpdateSpeed()
    {
        // Compute horizontal movement speed
        Vector3 flatVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z);
        float speed = flatVelocity.magnitude;

        // Use StringBuilder for efficient string formatting
        stringBuilder.Clear();
        stringBuilder.AppendFormat("Speed: {0:F2} u/s", speed);
        speedText.text = stringBuilder.ToString();
    }

    private void UpdateState()
    {
        // Determine state based on velocity and controller flags
        PlayerState state = DeterminePlayerState();
        stateText.text = $"State: {state}";
    }

    private PlayerState DeterminePlayerState()
    {
        if (!controllerScript.IsGrounded)
            return PlayerState.Jumping;
        if (controllerScript.IsCrouching)
            return PlayerState.Crouching;
        if (Input.GetKey(KeyCode.LeftShift) && playerRb.linearVelocity.magnitude > 0.1f)
            return PlayerState.Running;
        if (playerRb.linearVelocity.magnitude > 0.1f)
            return PlayerState.Walking;

        return PlayerState.Idle;
    }

    private enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Crouching,
        Jumping
    }
}
