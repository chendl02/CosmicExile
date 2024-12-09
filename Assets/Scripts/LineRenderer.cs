using UnityEngine;
public class SmoothCurvedLineBetweenPoints : MonoBehaviour
{
    public Transform endPoint; // 固定点的位置
    public int segmentCount; // 线段的分段数
    public float curveAmount; // 弯曲程度
    private LineRenderer lineRenderer;
    private Transform startPoint;
    private Vector3[] controlPoints;
    private int count;
    private bool isfixed;
    private Vector3 sphereCenter = Vector3.zero;
    public float sphereRadius = 184f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // 查找名为“Rocket.01”的对象并将其设置为startPoint
        GameObject rocketObject = GameObject.Find("Rocket.01");
        if (rocketObject != null)
        {
            startPoint = rocketObject.transform;
        }
        else
        {
            Debug.LogError("找不到名为Rocket.01的对象");
        }
        // 初始化控制点数组
        segmentCount = 200;
        curveAmount = 0.1f;
        controlPoints = new Vector3[segmentCount + 2];
        lineRenderer.positionCount = segmentCount + 2; // 设置线段的点数
        lineRenderer.startWidth = 0.2f; // 设置起点宽度
        lineRenderer.endWidth = 0.2f;
        count = 0;
        isfixed = false;
    }

    Vector3 CatmullRomSpline(float t, Vector3[] points)
    {
        int numSections = points.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
        float u = t * numSections - currPt;

        Vector3 a = points[currPt];
        Vector3 b = points[currPt + 1];
        Vector3 c = points[currPt + 2];
        Vector3 d = points[currPt + 3];

        return 0.5f * (
            (-a + 3f * b - 3f * c + d) * (u * u * u) +
            (2f * a - 5f * b + 4f * c - d) * (u * u) +
            (-a + c) * u +
            2f * b
        );
    }

    Vector3 GetPointOnSphereSurface(Vector3 point)
    {
        Vector3 direction = (point - sphereCenter).normalized;
        return sphereCenter + direction * sphereRadius;
    }

    void CreateNewLine()
    { // 创建一个新的空对象并添加LineRenderer组件
        GameObject newLineObject = new GameObject("NewLineRendererObject");
        LineRenderer newLineRenderer = newLineObject.AddComponent<LineRenderer>();
        // 复制当前LineRenderer的设置
        newLineRenderer.positionCount = lineRenderer.positionCount;
        newLineRenderer.startWidth = lineRenderer.startWidth;
        newLineRenderer.endWidth = lineRenderer.endWidth;
        newLineRenderer.material = lineRenderer.material;
        // 复制当前LineRenderer的点
        for (int i = 0; i < lineRenderer.positionCount; i++)
        { newLineRenderer.SetPosition(i, lineRenderer.GetPosition(i)); }
        // 重置当前
        lineRenderer.positionCount = segmentCount + 2;
        isfixed = false; // 重置标记
                      }

        void Update()
        {
            if (Input.GetKeyDown("p"))
            {
                isfixed = true;
                Debug.Log("Press P");
                CreateNewLine();
            }
            else if (isfixed == false)
            {

                GameObject playerObject = GameObject.Find("Player");
                if (playerObject != null)
                {
                    endPoint = playerObject.transform;
                }
                else
                {
                    Debug.LogError("找不到名为Player的对象");
                }
                if (count < 60) { count++; }
                else
                {
                    count = 0;
                    if (startPoint != null && endPoint != null)
                    {
                        // 设置起点和终点位置
                        Vector3 startPointOnSurface = GetPointOnSphereSurface(startPoint.position);
                        Vector3 endPointOnSurface = GetPointOnSphereSurface(endPoint.position);
                        lineRenderer.SetPosition(0, startPointOnSurface);
                        lineRenderer.SetPosition(segmentCount + 1, endPointOnSurface);

                        // 设置中间控制点位置
                        for (int i = 1; i <= segmentCount; i++)
                        {
                            float t = i / (float)(segmentCount + 1);
                            Vector3 randomOffset = new Vector3(
                                Random.Range(-curveAmount, curveAmount),
                                Random.Range(-curveAmount, curveAmount),
                                Random.Range(-curveAmount, curveAmount)
                            );
                            Vector3 pointPosition = Vector3.Lerp(startPointOnSurface, endPointOnSurface, t) + randomOffset;
                            pointPosition = GetPointOnSphereSurface(pointPosition);
                            lineRenderer.SetPosition(i, pointPosition);
                        }
                    }
                }
            }
        }
    }
