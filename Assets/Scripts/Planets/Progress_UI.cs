using UnityEngine;
using UnityEngine.UI;

public class Progress_UI : MonoBehaviour
{
    [SerializeField] private Scrollbar progressBar; // 进度条
    private int totalParts = 4;       // 总零件数量
    private int collectedParts = 0;   // 已收集的零件数量

    void Start()
    {
        // 初始化进度条为 0
        if (progressBar != null)
        {
            progressBar.size = 0f;
        }
    }

    // 增加零件进度
    public void AddPart()
    {
        if (collectedParts < totalParts)
        {
            collectedParts++;
            UpdateProgress();
            Debug.Log($"Parts Collected: {collectedParts}/{totalParts}");

            // 如果零件收集完毕
            if (collectedParts >= totalParts)
            {
                Debug.Log("All parts collected! Rocket is ready to launch.");
            }
        }
    }

    // 更新进度条
    private void UpdateProgress()
    {
        if (progressBar != null)
        {
            progressBar.size = (float)collectedParts / totalParts;
        }
    }
}
