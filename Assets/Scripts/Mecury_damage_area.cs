using UnityEngine;

public class PlaneDistanceCalculator : MonoBehaviour
{
    public Transform lightSource;    // 光源的 Transform
    public Transform character;     // 人物的 Transform

    [SerializeField] private int maxHealth = 100; // 最大血量
    private int currentHealth; // 当前血量

   void Start()
    {
        // 初始化当前血量
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 光线的方向（法向量）
        Vector3 lightDirection = -lightSource.forward.normalized;

        // 平面上的点（Z 轴上的点）
        Vector3 pointOnPlane = new Vector3(0, 0, 0);

        // 平面参数：a, b, c, d
        float a = lightDirection.x;
        float b = lightDirection.y;
        float c = lightDirection.z;
        float d = -(a * pointOnPlane.x + b * pointOnPlane.y + c * pointOnPlane.z);

        // 人物的位置
        Vector3 characterPosition = character.position;

        // 点到平面的距离公式
        float distance = Mathf.Abs(a * characterPosition.x + b * characterPosition.y + c * characterPosition.z + d)
                         / Mathf.Sqrt(a * a + b * b + c * c);

        //Debug.Log($"Distance to plane: {distance}");

        // 测试：按 J 键减少 10 点血量
        if (distance>20.0f)
        {
            Debug.Log($"Distance to plane: {distance}");
            ReduceHealth(10);
        }

        // 测试：按 H 键增加 10 点血量
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddHealth(10);
        }


    }

    // 减少血量
    public void ReduceHealth(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 确保血量不低于 0
        Debug.Log($"Health reduced by {amount}. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 增加血量
    public void AddHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 确保血量不超过最大值
        Debug.Log($"Health increased by {amount}. Current health: {currentHealth}");
    }

    // 玩家碰到其他物体时减少血量
    private void OnTriggerEnter(Collider other)
    {
        // 检测是否碰到了带有特定标签的物体，例如病毒
        if (other.CompareTag("Virus"))
        {
            Debug.Log($"Player collided with {other.gameObject.name}, taking damage.");

            // 调用减少血量的方法
            ReduceHealth(20);

            // 销毁碰到的病毒 Prefab
            Destroy(other.gameObject);
        }
    }

    // 处理玩家死亡逻辑
    private void Die()
    {
        Debug.Log("Player is dead!");
        // 添加死亡逻辑，例如重启关卡或显示游戏结束界面
    }

    // 获取当前血量（可选）
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}