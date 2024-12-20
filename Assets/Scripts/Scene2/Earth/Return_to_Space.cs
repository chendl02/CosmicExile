using UnityEngine;
using UnityEngine.SceneManagement;

public class Return_to_Space : MonoBehaviour
{
    public Transform player;               // 玩家角色
    public Vector3 targetPosition;         // 切换场景的目标位置
    public float triggerDistance = 3.0f;   // 距离阈值
    public string targetSceneName;         // 目标场景名称

    private bool isPlayerNear = false;     // 玩家是否靠近目标点
    private bool canEnterRocket = false;   // 是否可以进入火箭

    private Fuel_UI fuelUI;                // 燃料 UI 的引用
    private Progress_UI progressUI;        // 零件进度 UI 的引用

    void Start()
    {
        // 获取 Fuel_UI 和 Progress_UI 的引用
        fuelUI = FindObjectOfType<Fuel_UI>();
        progressUI = FindObjectOfType<Progress_UI>();

        if (fuelUI == null || progressUI == null)
        {
            Debug.LogError("Fuel_UI or Progress_UI not found in the scene!");
        }
    }

    void Update()
    {
        // 计算玩家与目标点的距离
        GameObject launchingPad = GameObject.Find("Launching Pad01");
        if (launchingPad != null)
        {
            targetPosition = launchingPad.transform.position;
        }

        float distance = Vector3.Distance(player.position, targetPosition);

        if (distance <= triggerDistance)
        {
            isPlayerNear = true;

            // 检测是否满足进入条件
            CheckProgress();

            // 如果按下 F 键
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (canEnterRocket)
                {
                    SwitchScene();
                }
                else
                {
                    Debug.Log("You need to collect enough parts and fuels of the rocket to restart it!");
                }
            }
        }
        else
        {
            isPlayerNear = false;
        }
    }

    // 检查 Fuel 和 Progress 的状态
    private void CheckProgress()
    {
        // 检查燃料和零件是否都已收集满
        if (fuelUI != null && progressUI != null)
        {
            canEnterRocket = fuelUIProgressIsComplete() && progressUIProgressIsComplete();
        }
    }

    // 燃料进度是否满
    private bool fuelUIProgressIsComplete()
    {
        return fuelUI.fuelProgressBar.size >= 1.0f; // 进度条满为 1.0f
    }

    // 零件进度是否满
    private bool progressUIProgressIsComplete()
    {
        return progressUI.progressBar.size >= 1.0f; // 进度条满为 1.0f
    }

    // 切换场景
    void SwitchScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }

    // 显示 UI 提示
    void OnGUI()
    {
        if (isPlayerNear)
        {
            // 创建自定义 GUIStyle
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 32; // 设置字体大小
            labelStyle.alignment = TextAnchor.MiddleCenter; // 设置文字居中对齐
            labelStyle.normal.textColor = Color.black; // 设置文字颜色为黑色
            // 计算提示框的位置和大小
            Rect labelRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 50, 400, 100);

            // 根据条件显示不同的提示
            if (canEnterRocket)
            {
                GUI.Label(labelRect, "Press F to enter the rocket: " + targetSceneName, labelStyle);
            }
            else
            {
                GUI.Label(labelRect, "You need to collect enough parts and fuels to restart the rocket!", labelStyle);
            }
        }
    }


    // 在场景视图中显示目标点的辅助线
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetPosition, triggerDistance);
    }
}
