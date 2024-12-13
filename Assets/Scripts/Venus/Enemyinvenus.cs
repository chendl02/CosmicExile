using UnityEngine;
using System.Collections.Generic;


public class EnemyAI : MonoBehaviour
{
    public Transform player; // 玩家对象
    public float detectionRange; // 检测范围
    public float moveSpeed; // 移动速度
    public Transform firePoint; // 发射点
    public float fireRate = 0.5f; // 发射频率
    private float nextFireTime = 0.1f; // 下一次发射时间
    public float sphereRadius = 184f;
    private Vector3 sphereCenter = Vector3.zero;
    void Start()
    {
        // 查找所有tag "fireball" 的预制体
        player = GameObject.Find("Player").transform;
        
        detectionRange = 30f;
        moveSpeed = 5f;
    }


void FixedUpdate()
    {
        // 计算敌人与玩家的距离
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 如果距离小于检测范围，开始追踪玩家并发射子弹
        if (distanceToPlayer < detectionRange)
        {
            // 追踪玩家
            if (distanceToPlayer >= 2f)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.position = GetPointOnSphereSurface(transform.position);
                transform.LookAt(player);
                transform.Rotate(0, 180, 0);
            }
            // 朝玩家发射子弹
            if (Time.time >= nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void FireBullet()
    {
        firePoint = transform;
        firePoint.position += transform.forward * 1f;
        // 创建一个会发光的球体
        GameObject enemySystem = GameObject.Find("EnemySystem");
        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.position = firePoint.position;
        bullet.transform.localScale = new Vector3(1, 1, 1);
        bullet.name = "Fireball";
        bullet.transform.parent = enemySystem.transform;

        // 添加发光效果
        Renderer renderer = bullet.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", Color.yellow);
        renderer.material = material;
        // 实例化子弹并设置其方向
        firePoint = transform; // 在物品朝向的方向上偏移 1 米
        //firePoint.position += transform.forward * 1f;

        Rigidbody rb = bullet.AddComponent<Rigidbody>();
        rb.velocity = (player.position - firePoint.position).normalized * 20f; // 设置子弹速度

        //rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Collider bulletCollider = bullet.GetComponent<Collider>(); 
        if (bulletCollider == null)
        {
            bulletCollider = bullet.AddComponent<SphereCollider>(); // 添加 SphereCollider
             }
            Destroy(bullet, 30f);
    }
    IEnumerator<WaitForSeconds> EnableBulletCollision(Collider bulletCollider)
    {
        yield return new WaitForSeconds(1f); // 延迟 0.1 秒
        bulletCollider.enabled = true;
    }
    Vector3 GetPointOnSphereSurface(Vector3 point)
    {
        // 计算点到球心的距离
        float distance = Vector3.Distance(point, sphereCenter);

        // 如果点在球外面，返回原点
        if (distance > sphereRadius)
        {
            return point;
        }

        // 如果点在球里面，计算球表面的点
        Vector3 direction = (point - sphereCenter).normalized;
        return sphereCenter + direction * sphereRadius;
    }

}

