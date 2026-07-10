using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public float baseScrollSpeed = 15f;
    public int maxChunksOnScreen = 5;
    public float chunkLength = 50f;
    public PlayerController player;

    private List<GameObject> activeChunks = new List<GameObject>();
    private float spawnZ = 0f;

    public float CurrentScrollSpeed { get; private set; }

    void Start()
    {
        // Erste Chunks zum Start generieren
        for (int i = 0; i < maxChunksOnScreen; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // Effektive Geschwindigkeit basierend auf dem Einlenk-Winkel berechnen
        float angleRad = player.currentRotationY * Mathf.Deg2Rad;
        CurrentScrollSpeed = baseScrollSpeed * Mathf.Cos(angleRad);

        // Bewege alle aktiven Chunks auf den Spieler zu (entgegen der Z-Achse)
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = activeChunks[i];
            chunk.transform.Translate(Vector3.back * CurrentScrollSpeed * Time.deltaTime, Space.World);

            // Wenn ein Chunk hinter den Spieler geraten ist, löschen und neuen vorne spawnen
            if (chunk.transform.position.z < -chunkLength)
            {
                activeChunks.RemoveAt(i);
                Destroy(chunk);
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        // Berechne die Position für den nächsten Chunk
        Vector3 spawnPos = new Vector3(0, 0, spawnZ);
        
        if (activeChunks.Count > 0)
        {
            // Setze den neuen Chunk direkt an das Ende des am weitesten entfernten Chunks
            float highestZ = -999f;
            foreach (var c in activeChunks)
            {
                if (c.transform.position.z > highestZ) highestZ = c.transform.position.z;
            }
            spawnPos = new Vector3(0, 0, highestZ + chunkLength);
        }

        GameObject newChunk = Instantiate(chunkPrefab, spawnPos, Quaternion.identity);
        activeChunks.Add(newChunk);
        
        // Erhöhe den Startwert für das allererste Setup
        spawnZ += chunkLength;
    }
}