using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private HealthManager healthManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) // 按J键造成10点伤害
        {
            healthManager.ReduceHealth(10);
        }

        if (Input.GetKeyDown(KeyCode.H)) // 按H键恢复10点血量
        {
            healthManager.AddHealth(10);
        }
    }
}