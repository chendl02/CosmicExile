using UnityEngine;

public class VirusController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 确保碰到的是玩家
        {
            Debug.Log($"Player collided with {gameObject.name}");

            // 获取 HealthManager 脚本，减少玩家血量
            HealthManager healthManager = FindObjectOfType<HealthManager>();
            if (healthManager != null)
            {
                healthManager.ReduceHealth(20); // 扣除 20 点血量
            }
            else
            {
                Debug.LogWarning("No HealthManager found!");
            }

            // 销毁病毒对象
            Destroy(gameObject);
        }
    }
}
