using UnityEngine;
using UnityEngine.InputSystem;

public class GordUltimate : MonoBehaviour
{
    [Header("Laser Settings")]
    public float maxDistance = 15f;
    public float tickDamage = 15f;    // Damage dealt per "tick"
    public float tickRate = 0.2f;     // How fast it ticks (0.2s = 5 times a second)

    [Header("References")]
    public Transform firePoint;       // Drag your LaserBeam object here
    public LayerMask groundLayer;
    
    private LineRenderer lineRenderer;
    private BoxCollider laserCollider;
    private bool isFiring = false;

    void Start()
    {
        // Get the components from the firePoint (LaserBeam object)
        lineRenderer = firePoint.GetComponent<LineRenderer>();
        laserCollider = firePoint.GetComponent<BoxCollider>();

        // Turn the laser off at the start of the game
        lineRenderer.enabled = false;
        laserCollider.enabled = false;
        
        // A line always needs 2 points: Start (0) and End (1)
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // 1. Start Firing (Mouse Button PRESSED)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isFiring = true;
            lineRenderer.enabled = true;
            laserCollider.enabled = true;
        }

        // 2. While Firing (Mouse Button HELD DOWN)
        if (isFiring && Mouse.current.leftButton.isPressed)
        {
            AimAndSweepLaser();
        }

        // 3. Stop Firing (Mouse Button RELEASED)
        if (isFiring && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isFiring = false;
            lineRenderer.enabled = false;
            laserCollider.enabled = false;
        }
    }

    void AimAndSweepLaser()
    {
        // --- AIMING LOGIC ---
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 targetPoint = hit.point;
            // Rotate the player to face the cursor
            Vector3 lookAtTarget = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
            transform.LookAt(lookAtTarget); 
        }

        // --- LINE RENDERER LOGIC ---
        // Point 0 is at the player. Point 1 is pushed directly forward.
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, firePoint.position + transform.forward * maxDistance);
    }
}