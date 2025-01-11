using UnityEngine;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour 
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private int initialBubbleCount = 20;
    
    [Header("Hemisphere Settings")]
    [SerializeField] private Vector3 hemisphereScale = Vector3.one;
    [SerializeField] private float radius = 10f;
    [SerializeField] private float transitionSpeed = 5f;
    
    [Header("Debug")]
    [SerializeField] private bool showSpawnArea = true;

    private class BubbleData
    {
        public GameObject bubble;
        public Vector3 normalizedPosition;
        public float currentRadius;
        public BubbleFloat floatComponent;
    }

    private List<BubbleData> bubbles = new List<BubbleData>();
    private float previousRadius;
    private Vector3 previousScale;

    private void Start()
    {
        previousRadius = radius;
        previousScale = hemisphereScale;
        SpawnInitialBubbles();
    }

    private void Update()
    {
        if (previousRadius != radius || previousScale != hemisphereScale)
        {
            AdaptBubblePositions();
            previousRadius = radius;
            previousScale = hemisphereScale;
        }

        UpdateBubblePositions();
    }

    private void SpawnInitialBubbles()
    {
        for (int i = 0; i < initialBubbleCount; i++)
        {
            SpawnBubble();
        }
    }

    public void SpawnBubble()
    {
        Vector3 normalizedPos = GetRandomHemispherePosition(1f);
        Vector3 scaledPos = Vector3.Scale(normalizedPos, hemisphereScale) * radius;
        Vector3 spawnPos = transform.position + scaledPos;

        GameObject bubble = Instantiate(bubblePrefab, spawnPos, Random.rotation);
        bubble.transform.parent = transform;

        // Setup BubbleFloat component
        BubbleFloat floatComponent = bubble.GetComponent<BubbleFloat>();
        if (floatComponent == null)
        {
            floatComponent = bubble.AddComponent<BubbleFloat>();
        }
        floatComponent.Initialize(spawnPos);

        bubbles.Add(new BubbleData
        {
            bubble = bubble,
            normalizedPosition = normalizedPos,
            currentRadius = radius,
            floatComponent = floatComponent
        });
    }

    private void AdaptBubblePositions()
    {
        foreach (var bubbleData in bubbles)
        {
            if (bubbleData.bubble == null) continue;

            Vector3 targetPos = Vector3.Scale(bubbleData.normalizedPosition, hemisphereScale) * radius;
            float currentDistance = Vector3.Distance(bubbleData.bubble.transform.position, transform.position);
            bubbleData.currentRadius = currentDistance;
        }
    }

    private void UpdateBubblePositions()
    {
        for (int i = bubbles.Count - 1; i >= 0; i--)
        {
            var bubbleData = bubbles[i];
            if (bubbleData.bubble == null)
            {
                bubbles.RemoveAt(i);
                continue;
            }

            Vector3 targetPos = transform.position + 
                Vector3.Scale(bubbleData.normalizedPosition, hemisphereScale) * radius;

            // Update the float component's target position
            if (bubbleData.floatComponent != null)
            {
                bubbleData.floatComponent.UpdateTargetPosition(targetPos);
            }

            float currentDistance = Vector3.Distance(bubbleData.bubble.transform.position, transform.position);
            if (currentDistance > radius * 1.5f)
            {
                // Add fade-out logic here if needed
            }
        }
    }

    private Vector3 GetRandomHemispherePosition(float normalizedRadius)
    {
        float theta = Random.Range(0f, Mathf.PI * 2);    // Azimuthal angle
        float phi = Random.Range(0f, Mathf.PI * 0.5f);   // Polar angle (half for hemisphere)
        
        // Convert to Cartesian coordinates on unit hemisphere
        float x = Mathf.Sin(phi) * Mathf.Cos(theta) * normalizedRadius;
        float y = Mathf.Cos(phi) * normalizedRadius;
        float z = Mathf.Sin(phi) * Mathf.Sin(theta) * normalizedRadius;
        
        return new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        if (!showSpawnArea) return;

        Gizmos.color = new Color(0.5f, 1f, 0.5f, 0.2f);
        DrawHemisphereGizmo();
    }

    private void DrawHemisphereGizmo()
    {
        int segments = 16;
        float stepSize = Mathf.PI * 0.5f / segments;
        
        for (int i = 0; i < segments; i++)
        {
            for (int j = 0; j < segments * 4; j++)
            {
                float phi1 = i * stepSize;
                float phi2 = (i + 1) * stepSize;
                float theta1 = j * stepSize;
                float theta2 = (j + 1) * stepSize;
                
                Vector3 p1 = GetHemispherePoint(phi1, theta1);
                Vector3 p2 = GetHemispherePoint(phi1, theta2);
                Vector3 p3 = GetHemispherePoint(phi2, theta1);
                
                Gizmos.DrawLine(transform.position + p1, transform.position + p2);
                Gizmos.DrawLine(transform.position + p1, transform.position + p3);
            }
        }
    }

    private Vector3 GetHemispherePoint(float phi, float theta)
    {
        float x = Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = Mathf.Cos(phi);
        float z = Mathf.Sin(phi) * Mathf.Sin(theta);
        return Vector3.Scale(new Vector3(x, y, z), hemisphereScale) * radius;
    }
}