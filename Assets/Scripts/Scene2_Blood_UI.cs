using UnityEngine;
using UnityEngine.UI;

public class Blood_UI : MonoBehaviour
{
    [SerializeField] private Scrollbar healthScrollbar; // 血量条
    private HealthManager healthManager;

    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>(); // 获取全局 HealthManager
        if (healthManager != null && healthScrollbar != null)
        {
            // 初始化血量条
            healthScrollbar.size = (float)healthManager.GetCurrentHealth() / healthManager.GetMaxHealth();
            
            // 订阅事件更新血量条
            healthManager.OnHealthChanged.AddListener(UpdateHealthBar);
        }
        else
        {
            Debug.LogWarning("HealthManager or HealthScrollbar not assigned!");
        }
    }

    private void UpdateHealthBar(int newHealth)
    {
        if (healthScrollbar != null)
        {
            // 更新血量条
            healthScrollbar.size = (float)newHealth / healthManager.GetMaxHealth();
            Debug.Log($"Health bar updated: {newHealth}");
        }
    }
}
