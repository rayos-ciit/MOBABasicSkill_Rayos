using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    private float currentHealth;
    
    [Header("Visuals")]
    public GameObject popupTextPrefab; // Drag your PopupText prefab here!
    
    private Renderer enemyRenderer;
    private Color originalColor;
    private bool isStunned = false;

    void Start()
    {
        currentHealth = maxHealth;
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null) originalColor = enemyRenderer.material.color;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // --- SPAWN DAMAGE TEXT ---
        ShowPopup(amount.ToString(), Color.yellow); 

        if (currentHealth <= 0) Die();
    }

    public void ApplyStun(float duration)
    {
        if (isStunned) return; // Don't stun if already stunned
        
        StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        
        // --- SPAWN STUN TEXT ---
        ShowPopup("STUNNED!", Color.cyan);
        
        if (enemyRenderer != null) enemyRenderer.material.color = Color.cyan;

        yield return new WaitForSeconds(duration);

        isStunned = false;
        if (enemyRenderer != null) enemyRenderer.material.color = originalColor;
    }

    void ShowPopup(string text, Color color)
    {
        if (popupTextPrefab != null)
        {
            // Spawn the text slightly above the enemy's head
            Vector3 spawnPos = transform.position + new Vector3(0, 2f, 0); 
            
            GameObject popup = Instantiate(popupTextPrefab, spawnPos, Quaternion.identity);
            
            // Get the script and set the message
            FloatingText floatingText = popup.GetComponent<FloatingText>();
            if (floatingText != null)
            {
                floatingText.Setup(text, color);
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}