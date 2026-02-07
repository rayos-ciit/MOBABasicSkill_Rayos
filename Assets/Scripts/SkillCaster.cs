using UnityEngine;
using UnityEngine.InputSystem; // REQUIRED for New Input System

public class SkillCaster : MonoBehaviour
{
    [Header("Skill Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public LayerMask groundLayer;

    private bool isAiming = false;
    private Vector3 targetPoint;

    void Update()
    {
        // 1. Check Mouse Button using New Input System
        // Mouse.current.leftButton.wasPressedThisFrame is the new "GetMouseButtonDown"
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isAiming = true;
        }

        // 2. While Aiming
        if (isAiming && Mouse.current.leftButton.isPressed)
        {
            AimAtCursor();
        }

        // 3. Fire (Button Released)
        if (isAiming && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            FireSkill();
            isAiming = false;
        }
    }

    void AimAtCursor()
    {
        // Get mouse position from New Input System
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, groundLayer))
        {
            targetPoint = hit.point;
            Vector3 lookAtTarget = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
            transform.LookAt(lookAtTarget);
        }
    }

    void FireSkill()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, transform.rotation);
        }
    }
}