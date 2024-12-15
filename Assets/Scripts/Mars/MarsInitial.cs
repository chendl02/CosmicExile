using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MarsInitial : MonoBehaviour
{
    public GameObject enemyAPrefab;
    public GameObject enemyBPrefab;
    public GameObject ironPrefab;
    public GameObject uraniumPrefab;
    public GameObject sandstormPrefab;

    public int numEnemies = 400;
    public int numIron = 100;
    public int numUranium = 50;
    public int numSandstorms = 5;
    public float planetRadius = 184f;

    void Start()
    {
        if (enemyAPrefab == null || enemyBPrefab == null || ironPrefab == null || uraniumPrefab == null)
        {
            Debug.LogError("One or more prefabs are not assigned!");
            return;
        }

        SpawnEnemies();
        SpawnMinerals(ironPrefab, numIron, "Iron");
        SpawnMinerals(uraniumPrefab, numUranium, "Uranium");
        SpawnSandstorms();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            Vector3 randomPositionA = GetRandomSurfacePoint(planetRadius);
            Vector3 randomPositionB = GetRandomSurfacePoint(planetRadius);
            Vector3 adjustedPositionA = MoveTowardsCenter(randomPositionA, 1);
            Vector3 adjustedPositionB = MoveTowardsCenter(randomPositionB, 1);

            GameObject enemyA = Instantiate(enemyAPrefab, adjustedPositionA, Quaternion.identity);
            enemyA.name = "ENEMY_A_" + i;
            enemyA.transform.up = randomPositionA.normalized;
            enemyA.transform.Rotate(0, Random.Range(0f, 360f), 0);

            GameObject enemyB = Instantiate(enemyBPrefab, adjustedPositionB, Quaternion.identity);
            enemyB.name = "ENEMY_B_" + i;
            enemyB.transform.up = randomPositionB.normalized;
            enemyB.transform.Rotate(0, Random.Range(0f, 360f), 0);
        }
    }

    bool IsNearOtherEnemies(Vector3 position, float minDistance)
    {
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in existingEnemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) < minDistance)
            {
                return true;
            }
        }
        return false;
    }

    void SpawnMinerals(GameObject mineralPrefab, int numMinerals, string mineralName)
    {
        for (int i = 0; i < numMinerals; i++)
        {
            Vector3 randomPosition = GetRandomSurfacePoint(planetRadius);
            Vector3 adjustedPosition = MoveTowardsCenter(randomPosition, 1f);
            GameObject mineral = Instantiate(mineralPrefab, adjustedPosition, Quaternion.identity);
            mineral.name = mineralName + "_" + i;
            mineral.tag = mineralName;
            mineral.transform.up = randomPosition.normalized;
            mineral.transform.Rotate(0, Random.Range(0f, 360f), 0);
        }
    }
    void SpawnSandstorms()
    {
        for (int i = 0; i < numSandstorms; i++)
        {
            Vector3 randomPosition = GetRandomSurfacePoint(planetRadius);
            Vector3 adjustedPosition = MoveTowardsCenter(randomPosition, 1f);
            GameObject sandstorm = Instantiate(sandstormPrefab, adjustedPosition, Quaternion.identity);
            sandstorm.name = "SANDSTORM_" + i;
            sandstorm.tag = "MarsStorm";
            sandstorm.transform.up = randomPosition.normalized;
            sandstorm.transform.Rotate(0, Random.Range(0f, 360f), 0);
        }
    }

    Vector3 GetRandomSurfacePoint(float radius)
    {
        Vector3 randomDirection = Random.onUnitSphere;
        return randomDirection * radius;
    }

    Vector3 MoveTowardsCenter(Vector3 position, float distance)
    {
        Vector3 directionToCenter = -position.normalized;
        Vector3 newPosition = position + directionToCenter * distance;
        float minRadius = planetRadius - planetRadius;
        return newPosition.magnitude < minRadius ? position : newPosition;
    }
}
