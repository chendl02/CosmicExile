using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Initial : MonoBehaviour
{
    public GameObject drugPrefab;
    public int numDrugs = 50;
    public LayerMask terrainLayer;

    public Collider terrainCollider;
    private string filePath = "/Users/censiyuan/Desktop/cs576/final_project/positions.txt";

    void Start()
    {
        if (terrainCollider == null)
        {
            Debug.LogError("Terrain Collider is not assigned!");
            return;
        }

        PrepareFile();
        SpawnDrugs();
    }

    void PrepareFile()
    {
        if (File.Exists(filePath))
        {
            File.WriteAllText(filePath, string.Empty);
        }
        else
        {
            Debug.Log($"File path does not exist. Creating file at: {filePath}");
        }
    }

    void SpawnDrugs()
    {
        HashSet<Vector3> usedPositions = new HashSet<Vector3>();
        List<Vector3> generatedPositions = new List<Vector3>();

        for (int i = 0; i < numDrugs; i++)
        {
            Vector3 randomPosition = GetRandomSurfacePoint();

            if (randomPosition != Vector3.zero && !usedPositions.Contains(randomPosition))
            {
                GameObject drug = Instantiate(drugPrefab, randomPosition, Quaternion.identity);
                drug.name = "DRUG_" + i;
                usedPositions.Add(randomPosition);
                generatedPositions.Add(randomPosition);

                Debug.Log($"Generated DRUG_{i} at position: {randomPosition}");
            }
            else
            {
                Debug.LogWarning($"Failed to find a valid position for drug {i}");
            }
        }

        Debug.Log("Starting to write positions to file...");
        WritePositionsToFile(generatedPositions);
    }

    Vector3 GetRandomSurfacePoint()
    {
        Bounds bounds = terrainCollider.bounds;

        Debug.Log($"Terrain Bounds: Min = {bounds.min}, Max = {bounds.max}");

        for (int attempts = 0; attempts < 10; attempts++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);

            Vector3 startPoint = new Vector3(x, bounds.max.y + 10f, z);

            if (Physics.Raycast(startPoint, Vector3.down, out RaycastHit hit, bounds.size.y * 2f, terrainLayer))
            {
                Debug.Log($"Raycast hit at: {hit.point}");
                return hit.point + Vector3.up * 0.5f;
            }
        }

        Debug.LogWarning("Failed to find a valid surface point");
        return Vector3.zero;
    }

    void WritePositionsToFile(List<Vector3> positions)
    {
        if (positions.Count == 0)
        {
            Debug.LogWarning("No positions to write to file");
            return;
        }

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            foreach (var pos in positions)
            {
                writer.WriteLine($"{pos.x}, {pos.y}, {pos.z}");
            }
        }
        Debug.Log($"Positions saved to {filePath}");
    }
}
