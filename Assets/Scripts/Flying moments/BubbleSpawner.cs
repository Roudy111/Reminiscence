using UnityEngine;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour 
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private int initialBubbleCount = 20;
    
    [Header("Volume Settings")]
    [SerializeField] private float spawnRadius = 10f;    // Horizontal spread
    [SerializeField] private float minHeight = 2f;       // Minimum spawn height
    [SerializeField] private float maxHeight = 15f;      // Maximum spawn height
    [SerializeField] private float centerBias = 0.5f;    // How much bubbles cluster toward center (0-1)
    
    [Header("Debug")]
    [SerializeField] private bool showSpawnArea = true;

    private void Start()
    {
        SpawnInitialBubbles();
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
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Random.rotation);
        bubble.transform.parent = transform;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Get random angle
        float angle = Random.Range(0f, Mathf.PI * 2);
        
        // Get random radius with center bias
        float randomValue = Random.value;
        float radius = spawnRadius * Mathf.Pow(randomValue, centerBias);
        
        // Calculate position with random height
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Lerp(minHeight, maxHeight, Random.value);
        float z = Mathf.Sin(angle) * radius;

        return transform.position + new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        if (!showSpawnArea) return;

        // Draw bottom circle
        Gizmos.color = new Color(0.5f, 1f, 0.5f, 0.2f);
        DrawCircle(transform.position + Vector3.up * minHeight, spawnRadius);

        // Draw top circle
        DrawCircle(transform.position + Vector3.up * maxHeight, spawnRadius);

        // Draw connecting lines
        int segments = 8;
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float x = Mathf.Cos(angle) * spawnRadius;
            float z = Mathf.Sin(angle) * spawnRadius;

            Vector3 bottom = transform.position + new Vector3(x, minHeight, z);
            Vector3 top = transform.position + new Vector3(x, maxHeight, z);
            Gizmos.DrawLine(bottom, top);
        }
    }

    private void DrawCircle(Vector3 center, float radius)
    {
        int segments = 32;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);
        
        for (int i = 1; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            Vector3 nextPoint = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}