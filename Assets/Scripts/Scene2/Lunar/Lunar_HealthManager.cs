using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // 最大血量
    private int currentHealth;

    // 血量变化事件，供UI等订阅
    public UnityEvent<int> OnHealthChanged;

    public string targetSceneName;     // 目标场景名称

    void Start()
    {
        currentHealth = maxHealth; // 初始化当前血量
        OnHealthChanged?.Invoke(currentHealth); // 通知初始血量
    }

    // 增加血量的方法
    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth); // 通知血量改变
    }

    // 减少血量的方法
    public void ReduceHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth); // 通知血量改变
        if (currentHealth <= 0)
        {
            HandleDeath(); // 血量为0时处理死亡
        }
    }

    // 获取当前血量
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    // 处理死亡逻辑
    private void HandleDeath()
    {
        Debug.Log("Player has died.");
        SwitchScene();

        // 可以添加死亡动画、游戏结束逻辑等
    }

    void SwitchScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}