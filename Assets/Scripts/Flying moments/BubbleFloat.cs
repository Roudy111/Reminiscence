using UnityEngine;

public class BubbleFloat : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float baseFloatSpeed = 0.3f;
    [SerializeField] private float verticalBias = 0.2f;
    [SerializeField] private float floatRange = 2f;
    
    [Header("Motion Settings")]
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float noiseScale = 0.5f;

    private Vector3 targetPosition;    // Target position from spawner
    private Vector3 randomOffset;
    private Vector3 randomRotation;
    private float timeOffset;
    private bool isInitialized = false;

    public void Initialize(Vector3 startPos)
    {
        targetPosition = startPos;
        randomOffset = Random.onUnitSphere * floatRange;
        timeOffset = Random.Range(0f, 100f);
        randomRotation = Random.onUnitSphere;
        isInitialized = true;
    }

    public void UpdateTargetPosition(Vector3 newTarget)
    {
        targetPosition = newTarget;
    }

    private void Update()
    {
        if (!isInitialized) return;

        float time = Time.time;

        // Calculate flow field motion
        float xFlow = Mathf.PerlinNoise(time * baseFloatSpeed + randomOffset.x, timeOffset) * 2 - 1;
        float yFlow = Mathf.PerlinNoise(timeOffset, time * baseFloatSpeed + randomOffset.y) * 2 - 1;
        float zFlow = Mathf.PerlinNoise(randomOffset.z, timeOffset + time * baseFloatSpeed) * 2 - 1;

        // Add upward bias
        yFlow += verticalBias;

        // Calculate float motion
        Vector3 flowMotion = new Vector3(xFlow, yFlow, zFlow) * floatRange;
        Vector3 desiredPosition = targetPosition + flowMotion;

        // Let the spawner handle the main position update
        transform.position = desiredPosition;

        // Handle rotation
        transform.Rotate(randomRotation * rotationSpeed * Time.deltaTime);

        // Scale pulsing
        float scale = 1f + Mathf.Sin(time * 0.5f + timeOffset) * 0.05f;
        transform.localScale = Vector3.one * scale;
    }
}