using UnityEngine;
using TMPro; // REQUIRED for TextMeshPro

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float destroyTime = 1f;
    
    // Change "TextMesh" to "TextMeshPro"
    private TextMeshPro textMesh; 

    void Awake()
    {
        // Get the TMP component instead of the old one
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(string textToDisplay, Color color)
    {
        textMesh.text = textToDisplay;
        textMesh.color = color;
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if (Camera.main != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}