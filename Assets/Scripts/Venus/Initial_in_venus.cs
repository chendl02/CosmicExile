using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initial_Venus : MonoBehaviour
{
    public GameObject CannonMachinePrefab; // Drug 的预制件
    public GameObject MachineGunRobotPrefab;
    public GameObject CablePrefeb;
    public int numEnemy = 30;     // 随机生成的 Enemy 数量
    public float planetRadius = 184f; // 球体半径固定为 184
    public int numCable = 200;

    void Start()
    {
        numEnemy = 30;
        numCable = 200;
        SpawnEnemy();
        SpawnTwinCable();
    }

    void SpawnEnemy()
    {
        GameObject enemySystem = GameObject.Find("EnemySystem");
        GameObject innerPlanet = GameObject.Find("InnerPlanet");
        for (int i = 0; i < numEnemy; i++)
        {
            // 根据固定半径生成表面点
            Vector3 randomPosition = GetRandomSurfacePoint(planetRadius, innerPlanet.transform.position);
            GameObject cannon = Instantiate(CannonMachinePrefab, randomPosition, Quaternion.identity);
            cannon.name = "Cannon_" + i;
            cannon.transform.parent = enemySystem.transform;

            randomPosition = GetRandomSurfacePoint(planetRadius, innerPlanet.transform.position);
            GameObject gunRobot = Instantiate(MachineGunRobotPrefab, randomPosition, Quaternion.identity);
            gunRobot.name = "GunRobot_" + i;
            gunRobot.transform.parent = enemySystem.transform;
        }
    }
    void SpawnTwinCable()
    {
        GameObject cableSystem = GameObject.Find("CableSystem");
        GameObject innerPlanet = GameObject.Find("TwinPlanet");
        for (int i = 0; i < numCable; i++)
        {
            // 根据固定半径生成表面点
            Vector3 randomPosition = GetRandomSurfacePoint(planetRadius, innerPlanet.transform.position);
            GameObject cable = Instantiate(CablePrefeb, randomPosition, Quaternion.identity);
            cable.name = "Cable_" + i;
            cable.transform.parent = cableSystem.transform;
        }
    }

    Vector3 GetRandomSurfacePoint(float radius, Vector3 originPoint)
    {
        // 生成随机单位方向向量
        Vector3 randomDirection = Random.onUnitSphere;

        // 根据固定半径计算表面点
        Vector3 surfacePoint = randomDirection * radius;

        return surfacePoint+originPoint;
    }
}
