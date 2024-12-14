using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Predict : MonoBehaviour
{
    public Toggle toggle;
    public Slider slider;
    public GameObject sliderObject;

    public static int initPredictDays = 100;

    public const int dayLimit = 1000;
    public const float deltaTime = Universe.physicsTimeStep * Universe.timeCoefficient;
    public Color lineColor;
    public LineRenderer lineRenderer; // 轨道的 LineRenderer
    public float lineWidth = 1.0f;
    private float emissionIntensity = 1.0f;

    private Queue<Vector3> trajectory;

    private int day;
    private int hour;
    private float dayTimeBegin;
    private float dayTimeEnd;
    private MotionData motionDataEnd;

    // Start is called before the first frame update
    void Start()
    {
        toggle.isOn = false;
        sliderObject.SetActive(false);

        trajectory = new Queue<Vector3>();

        toggle.onValueChanged.AddListener(togglePredict);


        // 确保有 LineRenderer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.enabled = false;

#if UNITY_EDITOR
        // 在编辑模式下使用 sharedMaterial 避免材质实例泄漏
        if (lineRenderer.sharedMaterial == null)
        {
            lineRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Color"));
        }
        lineRenderer.sharedMaterial.color = lineColor;
        lineRenderer.sharedMaterial.EnableKeyword("_EMISSION");
        lineRenderer.sharedMaterial.SetColor("_EmissionColor", lineColor * emissionIntensity);
#else
        // 在运行时可以安全地使用 material
        if (lineRenderer.material == null)
        {
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        }
        lineRenderer.material.color = lineColor;
        lineRenderer.material.EnableKeyword("_EMISSION");
        lineRenderer.material.SetColor("_EmissionColor", lineColor * emissionIntensity);
#endif


        // 设置 LineRenderer 的参数

        lineRenderer.startWidth = lineWidth; // 轨道宽度
        lineRenderer.endWidth = lineWidth;
    }

    void UpdateTrajectory(MotionData data)
    {
        float dayTimeNow = Clock.dayTime;
        float dayTime = dayTimeNow;
        day = 0;
        hour = 2;
        trajectory.Clear();
        do
        {
            data = NBodySimulation.RK4(data, dayTime, deltaTime);
            dayTime += deltaTime / 3600 / 24;
            if (Mathf.Floor((dayTime - dayTimeNow - day)*24) >= hour)
            {
                hour += 1;
                trajectory.Enqueue(data.Position);
            }
            if (hour >= 24)
            {
                hour = 0;
                day += 1;
            }
        }
        while (dayTime <= dayTimeNow + dayLimit + 1);
        dayTimeBegin = dayTimeNow;
        dayTimeEnd = dayTime;
        motionDataEnd = data;
    }

    public void setRenderer(int days)
    {
        int hours = days * 24;
        lineRenderer.positionCount = hours;
        lineRenderer.SetPositions(trajectory.Take(hours).ToArray());
        NBodySimulation.ActivateVirtualMesh(Clock.dayTime + days);
        NBodySimulation.DrawPredict(days);
    }

    void togglePredict(bool isOn)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (isOn)
        {
            if (Clock.speed != 0)
            {
                Clock clock = FindObjectOfType<Clock>();
                clock.pressPause();
            }

            Ship ship = FindObjectOfType<Ship>();
            UpdateTrajectory(ship.motionData);
            setRenderer(NonLinearSlider.previousValidValue);
            lineRenderer.enabled = true;
            sliderObject.SetActive(true);
        }
        else
        {
            lineRenderer.enabled = false;
            sliderObject.SetActive(false);
            NBodySimulation.DeactivateVirtualMesh();
            NBodySimulation.DrawOrbit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Clock.speed == 0)
            return;
        if (lineRenderer.enabled == false)
            return;
        if (Mathf.Floor((dayTimeEnd - dayTimeBegin - day)*24) > hour)
        {
            hour += 1;
            trajectory.Enqueue(motionDataEnd.Position);
            trajectory.Dequeue();
            setRenderer(NonLinearSlider.previousValidValue);
            
        }
        if (hour >= 24)
        {
            hour = 0;
            day += 1;
            NBodySimulation.UpdatePredict(dayTimeEnd);
        }
        motionDataEnd = NBodySimulation.RK4(motionDataEnd, dayTimeEnd, deltaTime);
        dayTimeEnd += deltaTime / 3600 / 24;
    }
}
