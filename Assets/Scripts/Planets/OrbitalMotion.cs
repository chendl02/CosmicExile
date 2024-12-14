using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMotion : MonoBehaviour
{
    // 半长轴 (a)，偏心率 (e)，周期 (T)
    public float semiMajorAxis;  // 半长轴
    public float eccentricity; // 偏心率
    public float period;  // 周期

    public float inclination; // 轨道倾角
    public float longitudeAscending; // 升交点经度
    public float argumentPerihelion; // 近拱点幅角

    public Vector3 initPos;
    public float initDistance;

    public Vector3 farPos;
    public float farDistance;

    public bool enableDrawer = true;
    public Color lineColor;
    public LineRenderer lineRenderer; // 轨道的 LineRenderer
    public float lineWidth = 1.0f;
    private int segments = 10000;       // 轨道的分段数
    private Vector3[] orbitPoints;


    private Queue<Vector3> trajectory;

    public CelestialBody orbitBody = null;


    // 计算在轨道上的位置
    public Vector2 GetOrbitalPosition(float dayTime)
    {
        float meanAnomaly = (2 * Mathf.PI / period) * (dayTime % period); // 平近点角 (M)
        float eccentricAnomaly = SolveKepler(meanAnomaly, eccentricity); // 偏近点角 (E)

        float fixedSemiMajorAxis = semiMajorAxis;

        if (orbitBody != null)
            fixedSemiMajorAxis += orbitBody.radius * 2;
        // 参数方程
        float x = fixedSemiMajorAxis * (Mathf.Cos(eccentricAnomaly) - eccentricity);
        float y = fixedSemiMajorAxis * Mathf.Sqrt(1 - eccentricity * eccentricity) * Mathf.Sin(eccentricAnomaly);

        return new Vector2(x, y);
    }

    // 开普勒方程迭代求解
    private float SolveKepler(float M, float e, int maxIterations = 100, float tolerance = 1e-6f)
    {
        float E = M; // 初始估计
        for (int i = 0; i < maxIterations; i++)
        {
            float deltaE = (E - e * Mathf.Sin(E) - M) / (1 - e * Mathf.Cos(E));
            E -= deltaE;
            if (Mathf.Abs(deltaE) < tolerance)
                break;
        }
        return E;
    }

    public Vector3 GetRealPosition(float dayTime)
    {
        // 获取二维轨道坐标
        Vector2 orbitalPosition = GetOrbitalPosition(dayTime);
        float x = orbitalPosition.x;
        float y = orbitalPosition.y;

        // 转换到三维空间
        float cosOmega = Mathf.Cos(longitudeAscending * Mathf.Deg2Rad);
        float sinOmega = Mathf.Sin(longitudeAscending * Mathf.Deg2Rad);
        float cosI = Mathf.Cos(inclination * Mathf.Deg2Rad);
        float sinI = Mathf.Sin(inclination * Mathf.Deg2Rad);
        float cosArg = Mathf.Cos(argumentPerihelion * Mathf.Deg2Rad);
        float sinArg = Mathf.Sin(argumentPerihelion * Mathf.Deg2Rad);

        // 变换矩阵
        float X = (cosOmega * cosArg - sinOmega * sinArg * cosI) * x + (-cosOmega * sinArg - sinOmega * cosArg * cosI) * y;
        float Y = (sinOmega * cosArg + cosOmega * sinArg * cosI) * x + (-sinOmega * sinArg + cosOmega * cosArg * cosI) * y;
        float Z = (sinArg * sinI) * x + (cosArg * sinI) * y;

        float rotatedX = -Y;
        float rotatedY = X;
        
        if (orbitBody != null)
            return orbitBody.Position + new Vector3(rotatedX, rotatedY, Z);
        else
            return new Vector3(rotatedX, rotatedY, Z);
    }


    public void DrawOrbit()
    {
        trajectory.Clear();
        // 计算轨道上的点
        if (orbitPoints == null)
        {
            orbitPoints = new Vector3[segments + 1];
            for (int i = 0; i < segments; i++)
            {
                orbitPoints[i] = GetRealPosition(i * period / segments);
            }
            orbitPoints[segments] = orbitPoints[0]; // 闭合点
        }

        // 设置 LineRenderer 的点
        lineRenderer.positionCount = segments + 1; // 分段数 + 闭合点
        lineRenderer.loop = true; // 闭合轨道
        lineRenderer.SetPositions(orbitPoints);
        
    }

    public void DrawPredict(int days)
    {
        if (trajectory.Count == 0)
        {
            for (int i = 1; i <= Predict.dayLimit; i++)
            {
                trajectory.Enqueue(GetRealPosition(Clock.dayTime + i));
            }
        }
        lineRenderer.positionCount = days; // 分段数 + 闭合点
        lineRenderer.loop = false; // 闭合轨道
        lineRenderer.SetPositions(trajectory.Take(days).ToArray());
    }

    public void updatePredict(float dayTime)
    {
        trajectory.Enqueue(GetRealPosition(dayTime));
        trajectory.Dequeue();
        DrawPredict(lineRenderer.positionCount);
    }

    void Start()
    {
        trajectory = new Queue<Vector3>();

        initPos = GetRealPosition(0);
        initDistance = initPos.magnitude;

        farPos = GetRealPosition(period / 2);
        farDistance = farPos.magnitude;

        // 确保有 LineRenderer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.enabled = enableDrawer;

#if UNITY_EDITOR
        // 在编辑模式下使用 sharedMaterial 避免材质实例泄漏
        if (lineRenderer.sharedMaterial == null)
        {
            lineRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Color"));
        }
        lineRenderer.sharedMaterial.color = lineColor;
#else
        // 在运行时可以安全地使用 material
        if (lineRenderer.material == null)
        {
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        }
        lineRenderer.material.color = lineColor;
#endif
        

        // 设置 LineRenderer 的参数
        
        lineRenderer.startWidth = lineWidth; // 轨道宽度
        lineRenderer.endWidth = lineWidth;

        DrawOrbit();
    }
}
