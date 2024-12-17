using UnityEngine;

public class FuelController : MonoBehaviour
{
    private Fuel_UI fuelUI; // 引用 Fuel_UI

    void Start()
    {
        // 查找场景中的 Fuel_UI
        fuelUI = FindObjectOfType<Fuel_UI>();
        if (fuelUI == null)
        {
            Debug.LogError("Fuel_UI not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 确保碰到的是玩家
        if (other.CompareTag("Player"))
        {
            Debug.Log("Fuel collected!");

            // 调用 Fuel_UI 更新进度条
            if (fuelUI != null)
            {
                fuelUI.AddFuel();
            }

            // 销毁自身
            Destroy(gameObject);
        }
    }
}
