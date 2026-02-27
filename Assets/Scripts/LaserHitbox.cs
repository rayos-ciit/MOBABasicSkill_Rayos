using UnityEngine;
using System.Collections.Generic;

public class LaserHitbox : MonoBehaviour
{
    private GordUltimate ultimateScript;
    
    // THE DICTIONARY: Tracks every enemy touched and remembers the exact time they last took damage.
    private Dictionary<Collider, float> hitTimers = new Dictionary<Collider, float>();

    void Start()
    {
        // Find the main script on the Player (the parent) to grab the damage/tick settings
        ultimateScript = GetComponentInParent<GordUltimate>();
    }

    // This runs EVERY SINGLE physics frame that an enemy is touching the beam
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 1. Have we met this enemy before? If not, add them to the dictionary!
            if (!hitTimers.ContainsKey(other))
            {
                hitTimers.Add(other, 0f); // 0f means they take damage immediately
            }

            // 2. Has enough time passed since their last hit?
            if (Time.time >= hitTimers[other] + ultimateScript.tickRate)
            {
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    // Deal the tick damage
                    enemy.TakeDamage(ultimateScript.tickDamage);
                    
                    // 3. Update their specific timer so they get a tiny break before the next burn
                    hitTimers[other] = Time.time; 
                }
            }
        }
    }

    // Clean up the dictionary when they walk out of the beam to prevent memory leaks
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hitTimers.ContainsKey(other))
            {
                hitTimers.Remove(other);
            }
        }
    }
    
    // Clean it out completely when the laser is turned off
    void OnDisable()
    {
        hitTimers.Clear();
    }
}