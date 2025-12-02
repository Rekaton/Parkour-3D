using Unity.VisualScripting;
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

    [Header("JumpVariables")]
    public float rangeFromWall = 5f;
    public float jumpSideForce;

    private Rigidbody rb;
    public bool isGrounded;               // Whether the AI is touching the ground
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
        JumpWhenNeeded();
        if (isGrounded)
        {
            MoveTowardsPlayer();
            //ChasePlayer();
        }

        //MoveTowardsPlayer();

        if (!canJump)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                canJump = true;
                Debug.Log("Jump ready again!");
            }
        }
    }

    private void ChasePlayer()
    {
        // Calculate direction to player
        Vector3 direction = (player.position - transform.position).normalized;
        // Move in that direction
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }


    public void JumpWhenNeeded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, rangeFromWall))
        {

            if (isGrounded && hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Found obstacle = jumping!");
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                rb.AddForce(Vector3.forward * jumpSideForce, ForceMode.Impulse);
            }
        }
    }


    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Lag en rotasjon mot spilleren
        Quaternion targetRot = Quaternion.LookRotation(direction);

        // Lås rotasjonen til kun Y-aksen
        Vector3 euler = targetRot.eulerAngles;
        euler.x = 0f;
        euler.z = 0f;
        targetRot = Quaternion.Euler(euler);

        // Slerp som før
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);

        // Bevegelse
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        //private void Jump()
        //{
        //    canJump = false;                     // Disable further jumps
        //    jumpTimer = jumpCooldown;            // Reset timer
        //    rb.AddForce((transform.forward + Vector3.up) * jumpForce, ForceMode.VelocityChange);
        //}

    }
}
