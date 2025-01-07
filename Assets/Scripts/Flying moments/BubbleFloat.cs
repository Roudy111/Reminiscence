using UnityEngine;

public class BubbleFloat : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float baseFloatSpeed = 0.3f;
    [SerializeField] private float verticalBias = 0.2f;  // Makes bubbles tend to float upward
    [SerializeField] private float floatRange = 2f;      // How far they can drift from their path
    
    [Header("Motion Settings")]
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float noiseScale = 0.5f;    // Scale of the flow field

    private Vector3 startPosition;
    private float randomOffset;
    private Vector3 randomRotation;
    private float timeOffset;

    private void Start()
    {
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 100f);
        timeOffset = Random.Range(0f, 100f);
        randomRotation = Random.onUnitSphere;
    }

    private void Update()
    {
        float time = Time.time;

        // Create a flow field effect using multiple Perlin noise samples
        float xFlow = Mathf.PerlinNoise(time * baseFloatSpeed + randomOffset, timeOffset) * 2 - 1;
        float yFlow = Mathf.PerlinNoise(timeOffset, time * baseFloatSpeed + randomOffset) * 2 - 1;
        float zFlow = Mathf.PerlinNoise(randomOffset, timeOffset + time * baseFloatSpeed) * 2 - 1;

        // Add upward bias to make bubbles tend to float up
        yFlow += verticalBias;

        // Calculate new position
        Vector3 flowMotion = new Vector3(xFlow, yFlow, zFlow) * floatRange;
        Vector3 newPosition = startPosition + flowMotion;

        // Smooth movement towards new position
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);

        // Gentle rotation
        transform.Rotate(randomRotation * rotationSpeed * Time.deltaTime);

        // Optional: Add subtle scale pulsing
        float scale = 1f + Mathf.Sin(time * 0.5f + randomOffset) * 0.05f;
        transform.localScale = Vector3.one * scale;
    }
}