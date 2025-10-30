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
        direction.y = rb.linearVelocity.y;
        // Move in that direction
        rb.linearVelocity = (transform.forward + direction * speed * Time.deltaTime);

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
        //MoveTowardsPlayer();
    }


    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
    }

}