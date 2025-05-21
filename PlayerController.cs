using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float crouchTransitionSpeed = 10f;

    private float originalHeight;
    private Rigidbody rb;
    private CapsuleCollider col;

    private bool isGrounded;
    private bool isCrouching;
    private bool canDoubleJump;

    public bool IsGrounded => isGrounded;
    public bool IsCrouching => isCrouching;

    private Vector3 inputDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        originalHeight = col.height;
    }

    void Update()
    {
        HandleInput();
        Crouch();
    }

    void FixedUpdate()
    {
        CheckGroundStatus();
        Move();
        Jump();
    }

    private void HandleInput()
    {
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
    }

    private void Move()
    {
        if (inputDirection == Vector3.zero) return;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * sprintMultiplier : speed;
        Vector3 move = transform.TransformDirection(inputDirection) * currentSpeed;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
        }
        else
        {
            rb.AddForce(move * 0.1f, ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = false;
            }
        }
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                StartCoroutine(SmoothCrouch(crouchHeight));
                isCrouching = true;
            }
        }
        else
        {
            if (isCrouching)
            {
                StartCoroutine(SmoothCrouch(originalHeight));
                isCrouching = false;
            }
        }
    }

    private System.Collections.IEnumerator SmoothCrouch(float targetHeight)
    {
        float currentHeight = col.height;
        while (Mathf.Abs(currentHeight - targetHeight) > 0.01f)
        {
            currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * crouchTransitionSpeed);
            col.height = currentHeight;
            yield return null;
        }
        col.height = targetHeight;
    }
}
