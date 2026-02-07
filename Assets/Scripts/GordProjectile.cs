using UnityEngine;

public class GordProjectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 10f;
    public float damage = 20f;
    public float stunDuration = 1.5f; // How long the stun lasts
    public float bounceHeight = 2.0f;
    public float bounceSpeed = 10f;
    public float lifetime = 3.0f;

    [Header("References")]
    public Transform visualBall;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (visualBall != null)
        {
            float newY = Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)) * bounceHeight;
            visualBall.localPosition = new Vector3(0, newY, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            
            if (enemy != null)
            {
                // 1. Deal Damage
                enemy.TakeDamage(damage);
                
                // 2. Apply Stun
                enemy.ApplyStun(stunDuration);
            }

            // 3. Destroy the ball immediately so it "pops"
            
            Debug.Log("crazy crazy crazy");
            
            Destroy(gameObject);
        }
    }
}