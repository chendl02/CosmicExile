using UnityEngine;
using UnityEngine.UI;

public class Fuel_UI : MonoBehaviour
{
    [SerializeField] private Scrollbar fuelProgressBar; // 进度条
    private int collectedFuel = 0; // 已收集燃料数量
    private int totalFuel = 5;     // 总燃料数量

    void Start()
    {
        // 初始化进度条为 0
        if (fuelProgressBar != null)
        {
            fuelProgressBar.size = 0;
        }
    }

    // 提供方法给其他脚本调用，增加燃料进度
    public void AddFuel()
    {
        if (collectedFuel < totalFuel)
        {
            collectedFuel++;
            UpdateFuelProgress();
            Debug.Log($"Fuel Progress: {collectedFuel}/{totalFuel}");
        }
    }

    private void UpdateFuelProgress()
    {
        // 更新进度条
        if (fuelProgressBar != null)
        {
            fuelProgressBar.size = (float)collectedFuel / totalFuel;
        }
    }
}
