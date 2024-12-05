using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public Transform player;           // 玩家角色
    public Vector3 targetPosition;     // 切换场景的目标位置
    public float triggerDistance = 3.0f; // 距离阈值
    public string targetSceneName;     // 目标场景名称

    private bool isPlayerNear = false; // 玩家是否靠近目标点
    private bool showUI = false;       // 是否显示切换提示 UI

    void Update()
    {
        // 计算玩家与目标点的距离
        float distance = Vector3.Distance(player.position, targetPosition);

        if (distance <= triggerDistance)
        {
            isPlayerNear = true;

            // 检测键盘输入触发场景切换
            if (Input.GetKeyDown(KeyCode.F))
            {
                SwitchScene();
            }
        }
        else
        {
            isPlayerNear = false;
        }
    }

    // 显示 UI 提示
    void OnGUI()
    {
        if (isPlayerNear)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 40), "按 F 键切换到场景：" + targetSceneName);
        }
    }

    // 切换场景
    void SwitchScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }

    // 在场景视图中显示目标点的辅助线
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetPosition, triggerDistance);
    }
}