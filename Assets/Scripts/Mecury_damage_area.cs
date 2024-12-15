using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlaneDistanceCalculator : MonoBehaviour
{
    public Transform lightSource;    // 光源的 Transform
    public Transform character;     // 人物的 Transform

    [SerializeField] private int maxHealth = 100; // 最大血量
    public int currentHealth; // 当前血量

    public float triggerValue = 20.0f;
    public Image redOverlay;

    public TextMeshProUGUI warningText;

    private HealthManager healthManager;

    public VehicleSwitch vehicleSwitch; // 引用角色切换脚本

   

   void Start()
    {
        // 初始化当前血量
        currentHealth = maxHealth;
        warningText.gameObject.SetActive(false);
        redOverlay.color = new Color(1, 0, 0, 0);

        healthManager = FindObjectOfType<HealthManager>(); // 获取全局 HealthManager

        InvokeRepeating("CheckDistance", 1f, 1f);  
    }

    void Update()
    {
        // 光线的方向（法向量）
        // Vector3 lightDirection = -lightSource.forward.normalized;

        // // 平面上的点（Z 轴上的点）
        // Vector3 pointOnPlane = new Vector3(0, 0, 0);

        // // 平面参数：a, b, c, d
        // float a = lightDirection.x;
        // float b = lightDirection.y;
        // float c = lightDirection.z;
        // float d = -(a * pointOnPlane.x + b * pointOnPlane.y + c * pointOnPlane.z);

        // // 人物的位置
        // Vector3 characterPosition = character.position;

        // 点到平面的距离公式
        // float distance = Mathf.Abs(a * characterPosition.x + b * characterPosition.y + c * characterPosition.z + d)
        //                  / Mathf.Sqrt(a * a + b * b + c * c);
        // float distance = (a * characterPosition.x + b * characterPosition.y + c * characterPosition.z + d)
        //                  / Mathf.Sqrt(a * a + b * b + c * c);

        // Debug.Log($"Distance to plane: {distance}");

        // 测试：按 J 键减少 10 点血量
        // if (distance>20.0f)
        // {
        //     Debug.Log($"Distance to plane: {distance}");
        //     ReduceHealth(10);
        // }

        // 测试：按 H 键增加 10 点血量
        if (Input.GetKeyDown(KeyCode.J))
        {
           ReduceHealth(10);
        }

        // if (distance > Distance)
        // {
        //     redOverlay.gameObject.SetActive(true);
        //     redOverlay.color = new Color(1, 0, 0, 0.5f); // 半透明红色
        //     warningText.gameObject.SetActive(true);
        //     warningText.text = "You are suffering from high temperature. Stay farther from sunlight.";
        // }
        // else if(distance < - triggerValue){
        //     redOverlay.gameObject.SetActive(true);
        //     redOverlay.color = new Color(0, 0, 1, 0.5f);
        //     warningText.gameObject.SetActive(true);
        //     warningText.text = "You are suffering from low temperature. Stay closer to sunlight.";
        // }
        // else
        // {
        //     redOverlay.color = new Color(1, 0, 0, 0);
        //     warningText.gameObject.SetActive(false);
        // }

    }

    public void CheckDistance()
    {
        bool isControllingTruck = vehicleSwitch.IsControllingTruck();
        // 光线的方向（法向量）
        Vector3 lightDirection = -lightSource.forward.normalized;

        Vector3 pointOnPlane = new Vector3(0, 0, 0);

        // 平面参数：a, b, c, d
        float a = lightDirection.x;
        float b = lightDirection.y;
        float c = lightDirection.z;
        float d = -(a * pointOnPlane.x + b * pointOnPlane.y + c * pointOnPlane.z);

        // 人物的位置
        Vector3 characterPosition = character.position;
        
        float distance = (a * characterPosition.x + b * characterPosition.y + c * characterPosition.z + d)
                         / Mathf.Sqrt(a * a + b * b + c * c);

        Debug.Log("distance: " + distance);

        if (distance > triggerValue && !isControllingTruck)
        {
            redOverlay.gameObject.SetActive(true);
            redOverlay.color = new Color(1, 0, 0, 0.5f); // 半透明红色
            warningText.gameObject.SetActive(true);
            warningText.text = "You are suffering from high temperature. Stay farther from sunlight or inside truck.";
            ReduceHealth(5);
        }
        else if(distance < - triggerValue && !isControllingTruck){
            redOverlay.gameObject.SetActive(true);
            redOverlay.color = new Color(0, 0, 1, 0.5f);
            warningText.gameObject.SetActive(true);
            warningText.text = "You are suffering from low temperature. Stay closer to sunlight or inside truck.";
            ReduceHealth(5);
        }
        else if(isControllingTruck)
        {
            redOverlay.color = new Color(1, 0, 0, 0);
            warningText.gameObject.SetActive(false);
            AddHealth(5);
        }
        else{
            redOverlay.color = new Color(1, 0, 0, 0);
            warningText.gameObject.SetActive(false);
        }
    }

    // 减少血量
    public void ReduceHealth(int amount)
    {
        healthManager.ReduceHealth(10);
        Debug.Log($"Health reduced by {amount}. Current health: {healthManager.GetCurrentHealth()}");
    }

    // 增加血量
    public void AddHealth(int amount)
    {
        healthManager.AddHealth(10);
        Debug.Log($"Health increased by {amount}. Current health: {healthManager.GetCurrentHealth()}");
    }

    // 玩家碰到其他物体时减少血量
    private void OnTriggerEnter(Collider other)
    {
        // 检测是否碰到了带有特定标签的物体，例如病毒
        if (other.CompareTag("Virus"))
        {
            Debug.Log($"Player collided with {other.gameObject.name}, taking damage.");

            // 调用减少血量的方法
            ReduceHealth(20);

            // 销毁碰到的病毒 Prefab
            Destroy(other.gameObject);
        }
    }

    // 处理玩家死亡逻辑
    private void Die()
    {
        Debug.Log("Player is dead!");
        // 添加死亡逻辑，例如重启关卡或显示游戏结束界面
    }

    // 获取当前血量（可选）
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}