using UnityEngine;
using UnityEngine.SceneManagement;

public class Lunar_to_Space : MonoBehaviour
{
    public Transform player;               // 玩家角色
    public Vector3 targetPosition;         // 切换场景的目标位置
    public float triggerDistance = 3.0f;   // 距离阈值
    public string targetSceneName;         // 目标场景名称

    private bool isPlayerNear = false;     // 玩家是否靠近目标点
    private bool canEnterRocket = false;   // 是否可以进入火箭

    public TaskHandler lunar_mission;

    // private Fuel_UI fuelUI;                // 燃料 UI 的引用
    // private Progress_UI progressUI;        // 零件进度 UI 的引用

    void Start()
    {
        // 获取 Fuel_UI 和 Progress_UI 的引用
        // fuelUI = FindObjectOfType<Fuel_UI>();
        // progressUI = FindObjectOfType<Progress_UI>();

        // if (fuelUI == null || progressUI == null)
        // {
        //     Debug.LogError("Fuel_UI or Progress_UI not found in the scene!");
        // }
    }

    void Update()
    {
        canEnterRocket = lunar_mission.isCompleted();
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

            // LunarOnGUI();
            // 如果按下 F 键
            if (Input.GetKeyDown(KeyCode.F))
            {
                
                // if (canEnterRocket)
                if(true)
                {
                    SwitchScene();
                }
                else
                {
                    Debug.Log("You need to collect enough water of the rocket to restart it!");
                }
            }
        }
        else
        {
            isPlayerNear = false;
        }
    }

    // 检查 Fuel 和 Progress 的状态
    // private void CheckProgress()
    // {
    //     // 检查燃料和零件是否都已收集满
    //     if (fuelUI != null && progressUI != null)
    //     {
    //         canEnterRocket = fuelUIProgressIsComplete() && progressUIProgressIsComplete();
    //     }
    // }

    // // 燃料进度是否满
    // private bool fuelUIProgressIsComplete()
    // {
    //     return fuelUI.fuelProgressBar.size >= 1.0f; // 进度条满为 1.0f
    // }

    // 零件进度是否满
    // private bool progressUIProgressIsComplete()
    // {
    //     return progressUI.progressBar.size >= 1.0f; // 进度条满为 1.0f
    // }

    // 切换场景
    void SwitchScene()
    {
        StageController.NextStage(4);
        
        SceneManager.LoadScene(targetSceneName);
    }

    // 显示 UI 提示
    void OnGUI()
    {
        if (isPlayerNear)
        {
            if (canEnterRocket)
            {
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 40), "Press F to enter the rocket: " + targetSceneName);
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 20, 300, 40), "You need to collect water to restart the rocket!");
            }
        }
    }

    // 在场景视图中显示目标点的辅助线
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(targetPosition, triggerDistance);
    // }
}