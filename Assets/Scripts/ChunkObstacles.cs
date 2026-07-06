using UnityEngine;

public class ChunkObstacles : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int numberOfObstacles = 3;
    public float roadWidth = 8f;
    public float chunkLength = 50f;

    void Start()
    {
        // Generiert zufällige Hindernisse auf diesem spezifischen Streckenteil
        for (int i = 0; i < numberOfObstacles; i++)
        {
            float randomX = Random.Range(-roadWidth, roadWidth);
            // Spawne Hindernisse verteilt über die Länge des Chunks
            float randomZ = Random.Range(-chunkLength / 2f, chunkLength / 2f); 

            Vector3 localSpawnPos = new Vector3(randomX, 1f, randomZ);
            
            // Instanziieren als Kindelement des Chunks, damit es sich mitbewegt
            GameObject obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.transform.localPosition = localSpawnPos;
        }
    }
}