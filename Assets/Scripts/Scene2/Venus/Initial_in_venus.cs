using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initial_Venus : MonoBehaviour
{
    public GameObject CannonMachinePrefab; // Drug ��Ԥ�Ƽ�
    public GameObject MachineGunRobotPrefab;
    public GameObject CablePrefeb;
    public int numEnemy = 30;     // ������ɵ� Enemy ����
    public float planetRadius = 184f; // ����뾶�̶�Ϊ 184
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
            // ���ݹ̶��뾶���ɱ����
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
            // ���ݹ̶��뾶���ɱ����
            Vector3 randomPosition = GetRandomSurfacePoint(planetRadius, innerPlanet.transform.position);
            GameObject cable = Instantiate(CablePrefeb, randomPosition, Quaternion.identity);
            cable.name = "Cable_" + i;
            cable.transform.parent = cableSystem.transform;
        }
    }

    Vector3 GetRandomSurfacePoint(float radius, Vector3 originPoint)
    {
        // ���������λ��������
        Vector3 randomDirection = Random.onUnitSphere;

        // ���ݹ̶��뾶��������
        Vector3 surfacePoint = randomDirection * radius;

        return surfacePoint+originPoint;
    }
}
