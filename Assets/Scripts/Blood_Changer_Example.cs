using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // 最大血量
    private int currentHealth; // 当前血量

    void Start()
    {
        // 初始化当前血量
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 测试：按 J 键减少 10 点血量
        if (Input.GetKeyDown(KeyCode.J))
        {
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
