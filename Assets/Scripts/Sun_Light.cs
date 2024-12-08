using UnityEngine;

public class OrbitingLight : MonoBehaviour
{
    public Transform mainPlanet;  // 主星球的位置（0, 0, 0）
    public float orbitRadius = 10f;  // 光源的轨道半径
    public float orbitSpeed = 20f;   // 光源的旋转速度（度/秒）

    private float angle = 0f;        // 当前角度

    void Update()
    {
        if (mainPlanet == null) return;

        // 计算光源在 XY 平面上的新位置
        angle += orbitSpeed * Time.deltaTime;
        float radian = angle * Mathf.Deg2Rad;

        float x = mainPlanet.position.x + orbitRadius * Mathf.Cos(radian);
        float y = mainPlanet.position.y + orbitRadius * Mathf.Sin(radian);
        float z = mainPlanet.position.z; // 光源在 XY 平面运动，Z 轴保持不变

        transform.position = new Vector3(x, y, z);

        // 光源方向始终指向主星球
        Vector3 direction = mainPlanet.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}