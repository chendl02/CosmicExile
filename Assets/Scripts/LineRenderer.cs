using UnityEngine;
public class SmoothCurvedLineBetweenPoints : MonoBehaviour
{
    public Transform endPoint; // �̶����λ��
    public int segmentCount; // �߶εķֶ���
    public float curveAmount; // �����̶�
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
        // ������Ϊ��Rocket.01���Ķ��󲢽�������ΪstartPoint
        GameObject rocketObject = GameObject.Find("Rocket.01");
        if (rocketObject != null)
        {
            startPoint = rocketObject.transform;
        }
        else
        {
            Debug.LogError("�Ҳ�����ΪRocket.01�Ķ���");
        }
        // ��ʼ�����Ƶ�����
        segmentCount = 200;
        curveAmount = 0.1f;
        controlPoints = new Vector3[segmentCount + 2];
        lineRenderer.positionCount = segmentCount + 2; // �����߶εĵ���
        lineRenderer.startWidth = 0.2f; // ���������
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
    { // ����һ���µĿն������LineRenderer���
        GameObject newLineObject = new GameObject("NewLineRendererObject");
        LineRenderer newLineRenderer = newLineObject.AddComponent<LineRenderer>();
        // ���Ƶ�ǰLineRenderer������
        newLineRenderer.positionCount = lineRenderer.positionCount;
        newLineRenderer.startWidth = lineRenderer.startWidth;
        newLineRenderer.endWidth = lineRenderer.endWidth;
        newLineRenderer.material = lineRenderer.material;
        // ���Ƶ�ǰLineRenderer�ĵ�
        for (int i = 0; i < lineRenderer.positionCount; i++)
        { newLineRenderer.SetPosition(i, lineRenderer.GetPosition(i)); }
        // ���õ�ǰ
        lineRenderer.positionCount = segmentCount + 2;
        isfixed = false; // ���ñ��
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
                    Debug.LogError("�Ҳ�����ΪPlayer�Ķ���");
                }
                if (count < 60) { count++; }
                else
                {
                    count = 0;
                    if (startPoint != null && endPoint != null)
                    {
                        // ���������յ�λ��
                        Vector3 startPointOnSurface = GetPointOnSphereSurface(startPoint.position);
                        Vector3 endPointOnSurface = GetPointOnSphereSurface(endPoint.position);
                        lineRenderer.SetPosition(0, startPointOnSurface);
                        lineRenderer.SetPosition(segmentCount + 1, endPointOnSurface);

                        // �����м���Ƶ�λ��
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
