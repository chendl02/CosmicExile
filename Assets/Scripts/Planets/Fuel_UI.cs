using UnityEngine;
using UnityEngine.UI;

public class Fuel_UI : MonoBehaviour
{
    [SerializeField] private Scrollbar fuelProgressBar; // 燃料进度条
    private int totalFuel = 10;          // 燃料总数量（改为10方便1/10计算）
    private int collectedFuel = 0;      // 当前已收集的燃料数量

    void Start()
    {
        // 初始化进度条为 0
        if (fuelProgressBar != null)
        {
            fuelProgressBar.size = 0f;
        }
    }

    // 增加燃料进度
    public void AddFuel()
    {
        if (collectedFuel < totalFuel)
        {
            collectedFuel++;
            UpdateFuelProgress();
            Debug.Log($"Fuel Collected: {collectedFuel}/{totalFuel}");
        }
    }

    // 按比例消耗燃料
    public void ConsumeFuel(int amount)
    {
        if (collectedFuel > 0)
        {
            collectedFuel -= amount;
            collectedFuel = Mathf.Clamp(collectedFuel, 0, totalFuel);
            UpdateFuelProgress();
            Debug.Log($"Fuel Consumed: {collectedFuel}/{totalFuel}");

            if (collectedFuel <= 0)
            {
                Debug.Log("Out of fuel!");
            }
        }
    }

    // 更新燃料进度条
    private void UpdateFuelProgress()
    {
        if (fuelProgressBar != null)
        {
            fuelProgressBar.size = (float)collectedFuel / totalFuel;
        }
    }

    // 检查燃料是否耗尽
    public bool IsFuelEmpty()
    {
        return collectedFuel <= 0;
    }
}
