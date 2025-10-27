using UnityEngine;

public class ChaseAiNoNavMesh : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    public float speed;
    [SerializeField]
    public float jumpForce;
    [SerializeField]
    public float groundCheckDistance;
    [SerializeField]
    public float jumpCooldown;
    public LayerMask groundMask;

    private Rigidbody rb;                  
    private bool isGrounded;               // Whether the AI is touching the ground
    private bool canJump = true; // flag that tracks if AI can jump
    private float jumpTimer;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Sends a ray straight down from the AI
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        // Calculate direction to player
        Vector3 direction = (player.position - transform.position).normalized;
        // Move in that direction
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

        // We shoot a ray forward from the AI's position
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1.5f))
        {
            // If it hits something tagged "Obstacle" and the AI is on the ground, jump
            if (isGrounded && hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Found = jumping");
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
        }

        JumpWhenNeeded();

    }

    public void JumpWhenNeeded()
    {
        // Shoot a ray forward to detect obstacles
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1.5f))
        {
            Debug.Log("Raycast hit: " + hit.collider.name + " | Tag: " + hit.collider.tag);

            // Check if it's an obstacle and we're grounded
            if (isGrounded && hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Found obstacle = jumping!");
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
        }
    }
    private void Jump()
    {
        canJump = false;                     // Disable further jumps
        jumpTimer = jumpCooldown;            // Reset timer
        rb.AddForce((transform.forward + Vector3.up) * jumpForce, ForceMode.VelocityChange);
    }

}
