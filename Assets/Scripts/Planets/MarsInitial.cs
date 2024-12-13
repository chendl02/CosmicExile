using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsInitial : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public int numEnemies = 200;    
    public float planetRadius = 184f; 

    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab is not assigned!");
            return;
        }

        SpawnDrugs();
    }

    void SpawnDrugs()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            // generate surface position
            Vector3 randomPosition = GetRandomSurfacePoint(planetRadius);
            Debug.Log($"Generated position of ENEMY_{i}: {randomPosition}");
            Vector3 adjustedPosition = MoveTowardsCenter(randomPosition, 1);
            // create Prefab
            GameObject enemy = Instantiate(enemyPrefab, adjustedPosition, Quaternion.identity);
            enemy.name = "ENEMY_" + i;

            // // look at center
            // drug.transform.LookAt(Vector3.zero);
            

            enemy.transform.up = randomPosition.normalized;  // Align to surface
            enemy.transform.Rotate(0, Random.Range(0f, 360f), 0);  // Add random rotation
        
        }
    }

    Vector3 GetRandomSurfacePoint(float radius)
    {
        Vector3 randomDirection = Random.onUnitSphere;

        Vector3 surfacePoint = randomDirection * radius;

        return surfacePoint;
    }
    Vector3 MoveTowardsCenter(Vector3 position, float distance)
    {
        Vector3 directionToCenter = -position.normalized;
        Vector3 newPosition = position + directionToCenter * distance;

        float minRadius = planetRadius - planetRadius;  // avoid cross surface
        return newPosition.magnitude < minRadius ? position : newPosition;
    }
}