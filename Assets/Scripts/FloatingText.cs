using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float destroyTime = 1f;
    
    private TMP_Text textMesh; // TMP_Text works for BOTH 3D and UI versions

    void Awake()
    {
        // Try to find ANY TextMeshPro component
        textMesh = GetComponent<TMP_Text>();
        
        if (textMesh == null) 
        {
            Debug.LogError("FloatingText Error: No TextMeshPro component found on this object!");
        }
    }

    public void Setup(string textToDisplay, Color color)
    {
        if (textMesh != null)
        {
            textMesh.text = textToDisplay;
            textMesh.color = color;
        }
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