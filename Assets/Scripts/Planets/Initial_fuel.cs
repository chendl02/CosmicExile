using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSpawner : MonoBehaviour
{
    public GameObject fuelPrefab;       // Fuel 的预制件
    public int numFuels = 100;            // 生成的 Fuel 数量
    public float planetRadius = 184f;   // 球体半径
    public float maxSpawnDistance = 300f; // Fuel 与玩家的最大生成距离
    public Vector3 playerPosition = new Vector3(0, 0, 0); // 玩家初始位置

    void Start()
    {
        if (fuelPrefab == null)
        {
            Debug.LogError("Fuel Prefab is not assigned!");
            return;
        }

        SpawnFuels();
    }

    void SpawnFuels()
    {
        int spawnedCount = 0;

        while (spawnedCount < numFuels)
        {
            // 根据球体半径生成随机表面点
            Vector3 randomPosition = GetRandomSurfacePoint(planetRadius);

            // 检查点与玩家初始位置的距离
            if (Vector3.Distance(randomPosition, playerPosition) <= maxSpawnDistance)
            {
                // 实例化 Fuel
                GameObject fuel = Instantiate(fuelPrefab, randomPosition, Quaternion.identity);
                fuel.name = "FUEL_" + spawnedCount;

                // 设置 Fuel 朝向地心
                fuel.transform.LookAt(Vector3.zero);

                // 计数 +1
                spawnedCount++;
            }
        }
    }

    Vector3 GetRandomSurfacePoint(float radius)
    {
        // 生成随机单位向量
        Vector3 randomDirection = Random.onUnitSphere;

        // 扩展到球体表面点
        Vector3 surfacePoint = randomDirection * radius;

        return surfacePoint;
    }
}
