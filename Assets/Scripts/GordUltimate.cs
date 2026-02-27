using UnityEngine;
using UnityEngine.InputSystem;

public class GordUltimate : MonoBehaviour
{
    [Header("Laser Settings")]
    public float maxDistance = 15f;
    public float tickDamage = 15f;    
    public float tickRate = 0.2f;     

    [Header("Ultimate Mechanics")]
    public float duration = 3.0f;      // How long the laser fires automatically
    public float cooldown = 2.0f;      // Wait time before you can fire again
    public float turnSpeed = 3.0f;     // Turn delay (Lower number = heavier/slower turn)

    [Header("References")]
    public Transform firePoint;       
    public LayerMask groundLayer;
    
    private LineRenderer lineRenderer;
    private BoxCollider laserCollider;
    
    // State Trackers
    private bool isFiring = false;
    private float fireTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        lineRenderer = firePoint.GetComponent<LineRenderer>();
        laserCollider = firePoint.GetComponent<BoxCollider>();

        lineRenderer.enabled = false;
        laserCollider.enabled = false;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // 1. Handle Cooldown Timer (Counts down to 0)
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // 2. Start Firing (Single Click)
        // We only fire IF we click, we aren't already firing, and cooldown is done
        if (Mouse.current.leftButton.wasPressedThisFrame && !isFiring && cooldownTimer <= 0)
        {
            StartFiring();
        }

        // 3. Handle Firing State
        if (isFiring)
        {
            // Count down the firing duration
            fireTimer -= Time.deltaTime;

            if (fireTimer <= 0)
            {
                StopFiring();
            }
            else
            {
                AimAndSweepLaser();
            }
        }
    }

    void StartFiring()
    {
        isFiring = true;
        fireTimer = duration; // Set the timer to 3 seconds
        
        lineRenderer.enabled = true;
        laserCollider.enabled = true;
        
        // When we first cast, we want to snap instantly to the mouse position
        SnapToInitialAim();
    }

    void StopFiring()
    {
        isFiring = false;
        cooldownTimer = cooldown; // Start the 2-second cooldown
        
        lineRenderer.enabled = false;
        laserCollider.enabled = false;
    }

    void SnapToInitialAim()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 targetPoint = hit.point;
            Vector3 lookAtTarget = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
            transform.LookAt(lookAtTarget); 
        }
        UpdateLaserVisuals();
    }

    void AimAndSweepLaser()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 targetPoint = hit.point;
            Vector3 lookAtTarget = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
            
            // Step A: Figure out the rotation we WANT to be at (pointing at the mouse)
            Quaternion targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
            
            // Step B: Smoothly rotate from our CURRENT rotation towards the TARGET rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        UpdateLaserVisuals();
    }

    void UpdateLaserVisuals()
    {
        // Always draw the laser extending directly out of the front of the player
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, firePoint.position + transform.forward * maxDistance);
    }
}