using UnityEngine;

public class BubbleFloat : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatHeight = 0.5f;
    [SerializeField] private float swayAmount = 0.5f;
    
    [Header("Random Motion")]
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float noiseSpeed = 0.5f;
    [SerializeField] private float noiseStrength = 0.5f;

    private Vector3 startPosition;
    private float randomOffset;
    private Vector3 randomRotation;

    private void Start()
    {
        // Store initial position
        startPosition = transform.position;
        
        // Random offset for each bubble to make them unique
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
        
        // Random rotation axis
        randomRotation = Random.onUnitSphere;
    }

    private void Update()
    {
        // Time-based variables
        float time = Time.time;
        
        // Gentle floating motion (up and down)
        float floatY = Mathf.Sin(time * floatSpeed + randomOffset) * floatHeight;
        
        // Swaying motion (side to side and front to back)
        float swayX = Mathf.Sin(time * 0.6f + randomOffset) * swayAmount;
        float swayZ = Mathf.Cos(time * 0.4f + randomOffset) * swayAmount;

        // Perlin noise for organic random motion
        float noiseX = (Mathf.PerlinNoise(time * noiseSpeed, randomOffset) - 0.5f) * noiseStrength;
        float noiseZ = (Mathf.PerlinNoise(randomOffset, time * noiseSpeed) - 0.5f) * noiseStrength;

        // Combine all motions
        Vector3 newPosition = startPosition + new Vector3(swayX + noiseX, floatY, swayZ + noiseZ);
        transform.position = newPosition;

        // Gentle rotation
        transform.Rotate(randomRotation * rotationSpeed * Time.deltaTime);

        // Optional: Add slight scale pulsing for more "magical" effect
        float scale = 1f + Mathf.Sin(time * 0.5f + randomOffset) * 0.05f;
        transform.localScale = Vector3.one * scale;
    }
}