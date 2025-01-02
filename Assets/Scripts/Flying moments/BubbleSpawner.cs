using UnityEngine;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour 
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private int initialBubbleCount = 20;
    
    [Header("Flow Path Settings")]
    [SerializeField] private int pathCount = 3;
    [SerializeField] private float pathRadius = 15f;
    [SerializeField] private float pathHeight = 10f;
    [SerializeField] private float noiseScale = 1f;
    [SerializeField] private float flowSpeed = 1f;
    
    private List<Vector3> spawnPoints = new List<Vector3>();
    private float time;

    private void Start()
    {
        SpawnInitialBubbles();
    }

    private void Update()
    {
        time += Time.deltaTime * flowSpeed;
        UpdateSpawnPoints();
    }

    private void UpdateSpawnPoints()
    {
        spawnPoints.Clear();
        
        for(int p = 0; p < pathCount; p++)
        {
            float pathOffset = (float)p / pathCount * Mathf.PI * 2;
            
            // Create flowing curves using sine waves and Perlin noise
            for(float t = 0; t < Mathf.PI * 2; t += 0.3f)
            {
                float x = Mathf.Sin(t + pathOffset + time) * pathRadius;
                float z = Mathf.Cos(t + pathOffset + time) * pathRadius;
                
                // Add organic movement using Perlin noise
                float noiseX = Mathf.PerlinNoise(t + time + pathOffset, 0) * noiseScale;
                float noiseZ = Mathf.PerlinNoise(0, t + time + pathOffset) * noiseScale;
                
                // Create a flowing up-and-down motion
                float y = Mathf.Sin(t * 2 + time + pathOffset) * pathHeight + pathHeight;
                
                Vector3 point = new Vector3(x + noiseX, y, z + noiseZ);
                spawnPoints.Add(transform.position + point);
            }
        }
    }

    private void SpawnInitialBubbles()
    {
        UpdateSpawnPoints();
        for (int i = 0; i < initialBubbleCount; i++)
        {
            SpawnBubble();
        }
    }

    public void SpawnBubble()
    {
        if(spawnPoints.Count == 0) return;
        
        // Pick a random point along the flow paths
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Random.rotation);
        bubble.transform.parent = transform;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        // Draw the flow paths
        Gizmos.color = Color.cyan;
        foreach(Vector3 point in spawnPoints)
        {
            Gizmos.DrawWireSphere(point, 0.3f);
        }
    }
}