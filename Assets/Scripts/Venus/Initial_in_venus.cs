using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initial_Venus : MonoBehaviour
{
    public GameObject CannonMachinePrefab; // Drug ��Ԥ�Ƽ�
    public GameObject MachineGunRobotPrefab;
    public int numEnemy = 30;     // ������ɵ� Drug ����
    public float planetRadius = 184f; // ����뾶�̶�Ϊ 184

    void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        GameObject enemySystem = GameObject.Find("EnemySystem");
        for (int i = 0; i < numEnemy; i++)
        {
            // ���ݹ̶��뾶���ɱ����
            Vector3 randomPosition = GetRandomSurfacePoint(planetRadius);
            GameObject cannon = Instantiate(CannonMachinePrefab, randomPosition, Quaternion.identity);
            cannon.name = "Cannon_" + i;
            cannon.transform.parent = enemySystem.transform;

            randomPosition = GetRandomSurfacePoint(planetRadius);
            GameObject gunRobot = Instantiate(MachineGunRobotPrefab, randomPosition, Quaternion.identity);
            gunRobot.name = "GunRobot_" + i;
            gunRobot.transform.parent = enemySystem.transform;
        }
    }

    Vector3 GetRandomSurfacePoint(float radius)
    {
        // ���������λ��������
        Vector3 randomDirection = Random.onUnitSphere;

        // ���ݹ̶��뾶��������
        Vector3 surfacePoint = randomDirection * radius;

        return surfacePoint;
    }
}
