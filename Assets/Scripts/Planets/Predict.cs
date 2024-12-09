using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Predict : MonoBehaviour
{
    public Toggle toggle;

    private int initPredictDays = 1000;

    public const int dayLimit = 10000;
    public const float deltaTime = Universe.physicsTimeStep * Universe.timeCoefficient;
    public Color lineColor;
    public LineRenderer lineRenderer; // ����� LineRenderer
    public float lineWidth = 1.0f;
    private int segments = 10000;       // ����ķֶ���
    private Vector3[] trajectory;

    // Start is called before the first frame update
    void Start()
    {
        toggle.isOn = false;

        trajectory = new Vector3[dayLimit + 1];

        toggle.onValueChanged.AddListener(togglePredict);


        // ȷ���� LineRenderer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.enabled = false;

#if UNITY_EDITOR
        // �ڱ༭ģʽ��ʹ�� sharedMaterial �������ʵ��й©
        if (lineRenderer.sharedMaterial == null)
        {
            lineRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Color"));
        }
        lineRenderer.sharedMaterial.color = lineColor;
#else
        // ������ʱ���԰�ȫ��ʹ�� material
        if (lineRenderer.material == null)
        {
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        }
        lineRenderer.material.color = lineColor;
#endif


        // ���� LineRenderer �Ĳ���
        
        lineRenderer.startWidth = lineWidth; // ������
        lineRenderer.endWidth = lineWidth;
    }

    void UpdateTrajectory(MotionData data)
    {
        float dayTimeNow = Clock.dayTime;
        float dayTime = dayTimeNow;
        int day = 0;
        trajectory[0] = data.Position;
        while(dayTime < dayTimeNow + dayLimit)
        {
            data = NBodySimulation.RK4(data, dayTime, deltaTime);
            dayTime += deltaTime / 3600 / 24;
            if (Mathf.Floor(dayTime - dayTimeNow) > day)
            {
                day += 1;
                trajectory[day] = data.Position;
            }
        }
    }

    void setRenderer(int days)
    {
        lineRenderer.positionCount = days + 1;
        Vector3[] points = new Vector3[days + 1];
        System.Array.Copy(trajectory, points, days + 1);
        lineRenderer.SetPositions(points);
    }

    void togglePredict(bool isOn)
    {
        if(isOn)
        {
            Ship ship = FindObjectOfType<Ship>();
            UpdateTrajectory(ship.motionData);
            setRenderer(initPredictDays);
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
