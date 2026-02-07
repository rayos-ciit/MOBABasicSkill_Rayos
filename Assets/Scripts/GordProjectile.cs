using UnityEngine;

public class GordProjectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 10f;
    public float damage = 20f;
    public float stunDuration = 1.5f;
    public float bounceHeight = 2.0f;
    public float bounceSpeed = 10f;
    public float lifetime = 3.0f;

    // We track the "flat" position separately so the bounce doesn't mess up forward movement
    private float startTime;
    private Vector3 startPosition;
    private Vector3 currentForwardPos;

    void Start()
    {
        startPosition = transform.position;
        currentForwardPos = startPosition;
        startTime = Time.time;
        
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 1. Calculate where we would be if we were just sliding on the ground (Forward)
        // We move the "virtual" position forward
        currentForwardPos += transform.forward * speed * Time.deltaTime;

        // 2. Calculate the Bounce (Up/Down)
        float bounceY = Mathf.Abs(Mathf.Sin((Time.time - startTime) * bounceSpeed)) * bounceHeight;

        // 3. Apply BOTH to the actual object
        // Position = Flat Position + Bounce Height
        transform.position = currentForwardPos + new Vector3(0, bounceY, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // --- INSTANT VISUAL POP ---
            // Hide the mesh immediately so it doesn't look like it's phasing through
            Renderer myRenderer = GetComponent<Renderer>();
            if (myRenderer != null) myRenderer.enabled = false;
            
            // Disable trail if you have one
            TrailRenderer trail = GetComponent<TrailRenderer>();
            if (trail != null) trail.enabled = false;

            // Stop logic
            speed = 0;
            GetComponent<Collider>().enabled = false;

            // --- DAMAGE & STUN ---
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                enemy.ApplyStun(stunDuration);
            }

            Debug.Log("POP! (Single Object)");
            Destroy(gameObject);
        }
    }
}