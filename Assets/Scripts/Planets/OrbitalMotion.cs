using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMotion : MonoBehaviour
{
    // �볤�� (a)��ƫ���� (e)������ (T)
    public float semiMajorAxis;  // �볤��
    public float eccentricity; // ƫ����
    public float period;  // ����

    public float inclination; // ������
    public float longitudeAscending; // �����㾭��
    public float argumentPerihelion; // ���������

    public Vector3 initPos;
    public float initDistance;

    public Vector3 farPos;
    public float farDistance;

    public bool enableDrawer = true;
    public Color lineColor;
    public LineRenderer lineRenderer; // ����� LineRenderer
    public float lineWidth = 1.0f;
    private int segments = 1000;       // ����ķֶ���


    // �����ڹ���ϵ�λ��
    public Vector2 GetOrbitalPosition(float time)
    {
        float meanAnomaly = (2 * Mathf.PI / period) * (time % period); // ƽ����� (M)
        float eccentricAnomaly = SolveKepler(meanAnomaly, eccentricity); // ƫ����� (E)

        // ��������
        float x = semiMajorAxis * (Mathf.Cos(eccentricAnomaly) - eccentricity);
        float y = semiMajorAxis * Mathf.Sqrt(1 - eccentricity * eccentricity) * Mathf.Sin(eccentricAnomaly);

        return new Vector2(x, y);
    }

    // �����շ��̵������
    private float SolveKepler(float M, float e, int maxIterations = 100, float tolerance = 1e-6f)
    {
        float E = M; // ��ʼ����
        for (int i = 0; i < maxIterations; i++)
        {
            float deltaE = (E - e * Mathf.Sin(E) - M) / (1 - e * Mathf.Cos(E));
            E -= deltaE;
            if (Mathf.Abs(deltaE) < tolerance)
                break;
        }
        return E;
    }

    public Vector3 GetRealPosition(float time)
    {
        // ��ȡ��ά�������
        Vector2 orbitalPosition = GetOrbitalPosition(time);
        float x = orbitalPosition.x;
        float y = orbitalPosition.y;

        // ת������ά�ռ�
        float cosOmega = Mathf.Cos(longitudeAscending * Mathf.Deg2Rad);
        float sinOmega = Mathf.Sin(longitudeAscending * Mathf.Deg2Rad);
        float cosI = Mathf.Cos(inclination * Mathf.Deg2Rad);
        float sinI = Mathf.Sin(inclination * Mathf.Deg2Rad);
        float cosArg = Mathf.Cos(argumentPerihelion * Mathf.Deg2Rad);
        float sinArg = Mathf.Sin(argumentPerihelion * Mathf.Deg2Rad);

        // �任����
        float X = (cosOmega * cosArg - sinOmega * sinArg * cosI) * x + (-cosOmega * sinArg - sinOmega * cosArg * cosI) * y;
        float Y = (sinOmega * cosArg + cosOmega * sinArg * cosI) * x + (-sinOmega * sinArg + cosOmega * cosArg * cosI) * y;
        float Z = (sinArg * sinI) * x + (cosArg * sinI) * y;

        float rotatedX = -Y;
        float rotatedY = X;

        return new Vector3(rotatedX, rotatedY, Z);
    }


    void DrawOrbit()
    {
        // �������ϵĵ�
        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i < segments; i++)
        {
            points[i] = GetRealPosition(i * period / segments);
        }
        points[segments] = points[0]; // �պϵ�

        // ���� LineRenderer �ĵ�
        lineRenderer.SetPositions(points);
    }

    void Start()
    {
        initPos = GetRealPosition(0);
        initDistance = initPos.magnitude;

        farPos = GetRealPosition(period / 2);
        farDistance = farPos.magnitude;

        // ȷ���� LineRenderer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.enabled = enableDrawer;

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
        lineRenderer.positionCount = segments + 1; // �ֶ��� + �պϵ�
        lineRenderer.loop = true; // �պϹ��
        lineRenderer.startWidth = lineWidth; // ������
        lineRenderer.endWidth = lineWidth;

        DrawOrbit();
    }
}
