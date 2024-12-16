using UnityEngine;

public class PartController : MonoBehaviour
{
    private Progress_UI progressUI; 
    public string partTag;   

    void Start()
    {
        // 查找场景中的 Progress_UI
        progressUI = FindObjectOfType<Progress_UI>();
        if (progressUI == null)
        {
            Debug.LogError("Progress_UI not found in the scene!");
        }

        // 检查是否正确设置了 Tag
        if (string.IsNullOrEmpty(partTag))
        {
            Debug.LogError("PartController: Part Tag not set!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 确保碰到的是玩家
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Part collected: {partTag}");

            // 调用 Progress_UI 更新进度条
            if (progressUI != null)
            {
                progressUI.AddPart();
            }

            // 销毁自身
            Destroy(gameObject);
        }
    }
}
