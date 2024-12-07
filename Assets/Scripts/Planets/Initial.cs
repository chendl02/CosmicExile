using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initial : MonoBehaviour
{
    public GameObject drugPrefab; // Drug 的预制件
    public int numDrugs = 200;     // 随机生成的 Drug 数量
    public float planetRadius = 184f; // 球体半径固定为 184

    void Start()
    {
        if (drugPrefab == null)
        {
            Debug.LogError("Drug Prefab is not assigned!");
            return;
        }

        SpawnDrugs();
    }

    void SpawnDrugs()
    {
        for (int i = 0; i < numDrugs; i++)
        {
            // 根据固定半径生成表面点
            Vector3 randomPosition = GetRandomSurfacePoint(planetRadius);
            Debug.Log($"Generated position of DRUG_{i}: {randomPosition}");

            // 动态实例化 Prefab
            GameObject drug = Instantiate(drugPrefab, randomPosition, Quaternion.identity);
            drug.name = "DRUG_" + i;

            // 设置对象朝向地心
            drug.transform.LookAt(Vector3.zero);
        }
    }

    Vector3 GetRandomSurfacePoint(float radius)
    {
        // 生成随机单位方向向量
        Vector3 randomDirection = Random.onUnitSphere;

        // 根据固定半径计算表面点
        Vector3 surfacePoint = randomDirection * radius;

        return surfacePoint;
    }
}
