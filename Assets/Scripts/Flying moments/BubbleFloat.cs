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

    [Header("Physics Settings")]
    [SerializeField] private float forceMultiplier = 5f;
    [SerializeField] private float damping = 1f;

    private Vector3 startPosition;
    private float randomOffset;
    private Vector3 randomRotation;
    private Rigidbody rb;

    private void Start()
    {
        // Cache rigidbody reference
        rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = false;
            rb.linearDamping = damping; // Updated from drag
            rb.angularDamping = damping; // Added for rotation damping
        }
        
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
        randomRotation = Random.onUnitSphere;
    }

    private void FixedUpdate()
    {
        if (!rb) return;

        float time = Time.time;
        
        // Calculate desired position
        float floatY = Mathf.Sin(time * floatSpeed + randomOffset) * floatHeight;
        float swayX = Mathf.Sin(time * 0.6f + randomOffset) * swayAmount;
        float swayZ = Mathf.Cos(time * 0.4f + randomOffset) * swayAmount;
        float noiseX = (Mathf.PerlinNoise(time * noiseSpeed, randomOffset) - 0.5f) * noiseStrength;
        float noiseZ = (Mathf.PerlinNoise(randomOffset, time * noiseSpeed) - 0.5f) * noiseStrength;

        Vector3 targetPosition = startPosition + new Vector3(swayX + noiseX, floatY, swayZ + noiseZ);
        
        // Apply force towards target position
        Vector3 moveDirection = (targetPosition - rb.position);
        rb.AddForce(moveDirection * forceMultiplier);

        // Apply rotation through physics
        rb.AddTorque(randomRotation * rotationSpeed * Time.fixedDeltaTime);

        // Scale pulsing
        float scale = 1f + Mathf.Sin(time * 0.5f + randomOffset) * 0.05f;
        transform.localScale = Vector3.one * scale;
    }
}