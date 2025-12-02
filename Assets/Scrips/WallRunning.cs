using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")] 
    public LayerMask Wall;
    public LayerMask Ground;
    public float wallRunForce;
    public float maxWallRunTime;
    public float wallRunTimer;
    public float wallJumpUPForce;
    public float wallJumpSideForce;

    [Header("Input")] 
    public KeyCode jumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;
    
    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;
    
    [Header("Detection")] 
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Refrences")] 
    public Transform orientation;
    private PlayerMovement pm;
    private Rigidbody rb;

    [Header("Exiting")] 
    private bool exitingWall;
    private float exitWallTimer;
    public float exitWallTime;
    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckForWalls();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(pm.wallRunning)
        {
            WallRunningMovement();
        }
    }
    private void CheckForWalls()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, Wall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, Wall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, Ground);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if ((wallLeft || wallRight) && verticalInput != 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallRunning)
            {
                StartWallRun();   
            }

            if (Input.GetKeyDown(jumpKey)) WallJump();
        }
        else if (exitingWall)
        {
            if (pm.wallRunning) StopWallRun();
            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if (exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }
        else
        {
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallRunning = true;
        
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal; 
        if (wallRight)
        {
            wallNormal = rightWallhit.normal;
        }
        else
        {
            wallNormal = leftWallhit.normal;
        }
        
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward + wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (useGravity)
        {
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }
    }
    
    
    private void StopWallRun()
    {
        pm.wallRunning = false;
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        Vector3 wallNormal;
        if (wallRight)
        {
            wallNormal = rightWallhit.normal;
        }
        else
        {
            wallNormal = leftWallhit.normal;
        }
        
        Vector3 forceToApply = transform.up * wallJumpUPForce + wallNormal * wallJumpSideForce;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
        
    }
}
