using UnityEngine;
using System.Collections.Generic;


public class EnemyAI : MonoBehaviour
{
    public Transform player; // ��Ҷ���
    public float detectionRange; // ��ⷶΧ
    public float moveSpeed; // �ƶ��ٶ�
    public Transform firePoint; // �����
    public float fireRate = 0.5f; // ����Ƶ��
    private float nextFireTime = 0.1f; // ��һ�η���ʱ��
    public float sphereRadius = 184f;
    private Vector3 sphereCenter = Vector3.zero;
    void Start()
    {
        // ��������tag "fireball" ��Ԥ����
        player = GameObject.Find("Player").transform;
        
        detectionRange = 30f;
        moveSpeed = 5f;
    }


void FixedUpdate()
    {
        // �����������ҵľ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �������С�ڼ�ⷶΧ����ʼ׷����Ҳ������ӵ�
        if (distanceToPlayer < detectionRange)
        {
            // ׷�����
            if (distanceToPlayer >= 2f)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.position = GetPointOnSphereSurface(transform.position);
                transform.LookAt(player);
                transform.Rotate(0, 180, 0);
            }
            // ����ҷ����ӵ�
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
        // ����һ���ᷢ�������
        GameObject enemySystem = GameObject.Find("EnemySystem");
        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.position = firePoint.position;
        bullet.transform.localScale = new Vector3(1, 1, 1);
        bullet.name = "Fireball";
        bullet.transform.parent = enemySystem.transform;

        // ��ӷ���Ч��
        Renderer renderer = bullet.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", Color.yellow);
        renderer.material = material;
        // ʵ�����ӵ��������䷽��
        firePoint = transform; // ����Ʒ����ķ�����ƫ�� 1 ��
        //firePoint.position += transform.forward * 1f;

        Rigidbody rb = bullet.AddComponent<Rigidbody>();
        rb.velocity = (player.position - firePoint.position).normalized * 20f; // �����ӵ��ٶ�

        //rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Collider bulletCollider = bullet.GetComponent<Collider>(); 
        if (bulletCollider == null)
        {
            bulletCollider = bullet.AddComponent<SphereCollider>(); // ��� SphereCollider
             }
            Destroy(bullet, 30f);
    }
    IEnumerator<WaitForSeconds> EnableBulletCollision(Collider bulletCollider)
    {
        yield return new WaitForSeconds(1f); // �ӳ� 0.1 ��
        bulletCollider.enabled = true;
    }
    Vector3 GetPointOnSphereSurface(Vector3 point)
    {
        // ����㵽���ĵľ���
        float distance = Vector3.Distance(point, sphereCenter);

        // ������������棬����ԭ��
        if (distance > sphereRadius)
        {
            return point;
        }

        // ������������棬���������ĵ�
        Vector3 direction = (point - sphereCenter).normalized;
        return sphereCenter + direction * sphereRadius;
    }

}

