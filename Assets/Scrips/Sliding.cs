using UnityEngine;

public class Sliding : MonoBehaviour
{

    [Header("References")] 
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")] 
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float sliderYScale;
    private float startYScale; 
    
    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        
        startYScale = playerObj.localScale.y;
    }
    
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput > 0 || verticalInput > 0))
        {
            StartSlide();
        }
        
        if (Input.GetKeyUp(slideKey) && pm.sliding) StopSlide();
        
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
        {
            SlidingMovement();
        }
    }
    private void StartSlide()
    {
        pm.sliding = true;
        
        playerObj.localScale = new Vector4(playerObj.localScale.x, sliderYScale,  playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if (!pm.OnSlope() || rb.linearVelocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
        
            slideTimer -= Time.fixedDeltaTime;
        }
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);

        }

        if (slideTimer <= 0.0f)
        {
            StopSlide();
        }
    }
    
    private void StopSlide()
    {
        pm.sliding = false;
        
        playerObj.localScale = new Vector4(playerObj.localScale.x, startYScale, playerObj.localScale.z);

    }
    
}
