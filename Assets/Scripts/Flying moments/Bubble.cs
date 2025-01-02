using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Bubble Properties")]
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.5f;

    [Header("Color Properties")]
    [SerializeField] private float alpha = 0.6f;  // For transparency

    private void Start()
    {
        // Random size for variety
        float randomScale = Random.Range(minScale, maxScale);
        transform.localScale = Vector3.one * randomScale;

        // Random color
        Color randomColor = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.7f, 1f);
        randomColor.a = alpha;  // Set transparency

        // Apply the color to the material
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            renderer.material.color = randomColor;
        }
    }
}