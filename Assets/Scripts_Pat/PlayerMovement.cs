using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float speed = 6f;
    public float walkSpeed;
    public float sprintSpeed;

    public Transform orientation;

    public float groundDrag = 5f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask groundLayer;
    bool isGrounded;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air,
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        MyInput();
        SpeedConstraint();

        // Apply drag correctly
        rb.linearDamping = isGrounded ? groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = 0f;
        verticalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) verticalInput += 1f;
            if (Keyboard.current.sKey.isPressed) verticalInput -= 1f;
            if (Keyboard.current.dKey.isPressed) horizontalInput += 1f;
            if (Keyboard.current.aKey.isPressed) horizontalInput -= 1f;
        }

        // Normalize diagonal movement
        Vector2 normalized = new Vector2(horizontalInput, verticalInput).normalized;
        horizontalInput = normalized.x;
        verticalInput = normalized.y;

        // Jump input
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void SetHandler()
    {
        if(isGrounded && Input.GetKeyDown(jumpKey))
    }
    private void MovePlayer()
    {
        // Flatten forward/right so player doesn’t tilt up/down
        Vector3 forward = orientation.forward;
        Vector3 right = orientation.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Combine input
        moveDirection = forward * verticalInput + right * horizontalInput;

        // Add force based on whether grounded or in air
        if (isGrounded)
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedConstraint()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset Y velocity before jump
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
