using System.Collections.Generic;
using UnityEngine;

public class ChunkObstacles : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public int numberOfObstacles = 3;
    public float roadWidth = 8f;
    public float chunkLength = 50f;

    [Header("Abstand zwischen Hindernissen")]
    public float minDistanceBetweenObstacles = 3f;
    public int maxSpawnAttempts = 20; // Verhindert Endlosschleife bei zu engem Platz

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("Keine Obstacle-Prefabs zugewiesen!");
            return;
        }
        
        // Generiert zufällige Hindernisse auf diesem spezifischen Streckenteil
        for (int i = 0; i < numberOfObstacles; i++)
        {
            Vector3 localSpawnPos;
            bool validPositionFound = TryFindValidPosition(out localSpawnPos);

            if (!validPositionFound)
            {
                // Kein gültiger Platz gefunden -> dieses Hindernis überspringen
                continue;
            }

            spawnedPositions.Add(localSpawnPos);
            
            // Zufälliges Prefab aus dem Array auswählen
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject prefabToSpawn = obstaclePrefabs[randomIndex];

            // Instanziieren als Kindelement des Chunks, damit es sich mitbewegt
            GameObject obstacle = Instantiate(prefabToSpawn, transform);
            obstacle.transform.localPosition = localSpawnPos;
        }
    }

    bool TryFindValidPosition(out Vector3 result)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            float randomX = Random.Range(-roadWidth, roadWidth);
            float randomZ = Random.Range(-chunkLength / 2f, chunkLength / 2f);
            Vector3 candidate = new Vector3(randomX, 0f, randomZ);

            bool tooClose = false;
            foreach (var existingPos in spawnedPositions)
            {
                // Nur horizontale Distanz prüfen (X und Z), Y ignorieren
                float distance = Vector2.Distance(
                    new Vector2(candidate.x, candidate.z),
                    new Vector2(existingPos.x, existingPos.z)
                );

                if (distance < minDistanceBetweenObstacles)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                result = candidate;
                return true;
            }
        }

        // Nach maxSpawnAttempts keinen gültigen Platz gefunden
        result = Vector3.zero;
        return false;
    }
}