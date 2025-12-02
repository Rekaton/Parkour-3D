using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;               
    public Rigidbody playerRb;       
    public float maxFov = 90f;       
    public float changeSpeed = 5f;   
    private float ogFov;             
    private float playerSpeed;       

    void Start()
    {
        ogFov = cam.fieldOfView; 
    }

    void Update()
    {
        playerSpeed = playerRb.linearVelocity.magnitude;
        float targetFov = Mathf.Lerp(ogFov, maxFov, playerSpeed / 20f); 
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * changeSpeed);
    }
}