using UnityEngine;
using UnityEngine.UI;

public class Blood_UI : MonoBehaviour
{
    [SerializeField] private Scrollbar healthScrollbar; // UI的ScrollBar组件
    [SerializeField] private HealthManager healthManager; // HealthManager组件引用

    void Start()
    {
        if (healthManager != null && healthScrollbar != null)
        {
            // 初始化ScrollBar值
            healthScrollbar.size = 1f; // 初始值设置为满血（1表示100%）
            healthManager.OnHealthChanged.AddListener(UpdateHealthBar); // 订阅事件
        }
    }

    private void UpdateHealthBar(int newHealth)
    {
        if (healthScrollbar != null)
        {
            // 将血量值映射到0.0~1.0
            float normalizedValue = Mathf.Clamp01((float)newHealth / healthManager.GetMaxHealth());
            healthScrollbar.size = normalizedValue;

            Debug.Log($"Health updated: {newHealth}, Normalized: {normalizedValue}");
        }
    }
}