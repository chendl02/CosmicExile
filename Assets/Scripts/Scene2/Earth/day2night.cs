using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Material daySkybox;            // 白天的天空盒材质
    public Material nightSkybox;          // 夜晚的天空盒材质
    public Light directionalLight;        // 方向光源，模拟太阳
    public float dayDuration = 10f;       // 白天与夜晚的总时长（秒）
    
    private float timeCounter = 0f;       // 用于追踪时间
    private bool isDay = true;            // 是否为白天

    void Start()
    {
        // 初始化：设置初始为白天
        if (directionalLight != null)
        {
            directionalLight.intensity = 1f; // 白天光照强度
        }

        RenderSettings.skybox = daySkybox;
    }

    void Update()
    {
        // 更新时间
        timeCounter += Time.deltaTime;

        // 切换昼夜
        if (timeCounter >= dayDuration)
        {
            ToggleDayNight();
            timeCounter = 0f; // 重置计时器
        }

        // 平滑旋转方向光源，模拟太阳移动
        if (directionalLight != null)
        {
            directionalLight.transform.Rotate(Vector3.right * (360 / dayDuration) * Time.deltaTime);
        }
    }

    private void ToggleDayNight()
    {
        isDay = !isDay;

        if (isDay)
        {
            // 切换到白天
            RenderSettings.skybox = daySkybox;
            if (directionalLight != null)
            {
                directionalLight.intensity = 1f; // 白天光照强度
            }
            Debug.Log("Switched to Daytime");
        }
        else
        {
            // 切换到夜晚
            RenderSettings.skybox = nightSkybox;
            if (directionalLight != null)
            {
                directionalLight.intensity = 0.01f; // 夜晚光照强度
            }
            Debug.Log("Switched to Nighttime");
        }
    }
}
