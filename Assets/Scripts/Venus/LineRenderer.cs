using System.Collections.Generic;
using UnityEngine;
public class CableSystemVenus : MonoBehaviour
{

    private Vector3 sphereCenter = Vector3.zero;
    public float sphereRadius = 184f;

    public List<Cable> cableList = new List<Cable>();
    public Cable currentCable;
    public int count=0;
    public Material lineMaterial;
    public Material lineMaterialFixed;

    void Start()
    {
        GameObject rocketObject = GameObject.Find("Rocket.01");
        if (rocketObject != null)
        {
            currentCable = new Cable("currentCable", rocketObject.transform);
        }
        else
        {
            Debug.LogError("ERROR!!! No Rocket!!!");
        }
        count = 0;
        if (lineMaterial == null) { Debug.LogError("ERROR!!! Unable to load Sun material."); }
        currentCable.lineRenderer.material = lineMaterial;
    }

    Vector3 GetPointOnSphereSurface(Vector3 point)
    {
        // 计算点到球心的距离
        float distance = Vector3.Distance(point, sphereCenter);

        // 如果点在球外面，返回原点
        if (distance > sphereRadius)
        {
            return point;
        }

        // 如果点在球里面，计算球表面的点
        Vector3 direction = (point - sphereCenter).normalized;
        return sphereCenter + direction * sphereRadius;
    }


    void Update()
    {
        GameObject playerObject = GameObject.Find("Player");
        float distance = Vector3.Distance(currentCable.startPoint.position, playerObject.transform.position);
        if (distance > 5000f)
        {
            currentCable.lineRenderer.enabled = false;
            return;
        }
        else { currentCable.lineRenderer.enabled = true; }
        if (Input.GetKeyDown("p"))
        {
            //store all fixed
            Debug.Log("Trigger");
            if (currentCable == null)
            {
                Debug.LogError("ERROR!!! currentCable is not initialized.");
                return;
            }
            if (currentCable.isfixed == false)
            {
                cableList.Add(currentCable);
                currentCable.isfixed = true;
                currentCable.lineRenderer.material = lineMaterialFixed;
            }
            Transform temp = new GameObject("TempTransform").transform;
            temp.position = cableList[cableList.Count - 1].endPoint.position;
            currentCable = new Cable("newCable", temp);
            currentCable.lineRenderer.material = lineMaterial;
            //Destroy(temp.gameObject);
        }
        
        if (playerObject != null)
        {
            currentCable.endPoint = playerObject.transform;
        }
        else
        {
            Debug.LogError("ERROR!!! No Player!!!");
        }
        
        //Debug.Log("new end:" + currentCable.endPoint.position);
        //if (count < 60) { count++; }
        //else
        {
            count = 0;
            if (currentCable.startPoint != null && currentCable.endPoint != null)
            {
                Vector3 startPointOnSurface = GetPointOnSphereSurface(currentCable.startPoint.position);
                Vector3 endPointOnSurface = GetPointOnSphereSurface(currentCable.endPoint.position);
                currentCable.lineRenderer.SetPosition(0, startPointOnSurface);
                currentCable.lineRenderer.SetPosition(currentCable.segmentCount + 1, endPointOnSurface);

                for (int i = 1; i <= currentCable.segmentCount; i++)
                {
                    float t = i / (float)(currentCable.segmentCount + 1);
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-currentCable.curveAmount, currentCable.curveAmount),
                        Random.Range(-currentCable.curveAmount, currentCable.curveAmount),
                        Random.Range(-currentCable.curveAmount, currentCable.curveAmount)
                    );
                    Vector3 pointPosition = Vector3.Lerp(startPointOnSurface, endPointOnSurface, t) + randomOffset;
                    pointPosition = GetPointOnSphereSurface(pointPosition);
                    currentCable.lineRenderer.SetPosition(i, pointPosition);
                }
            }
        }
        }
        }

public class Cable
{
    public GameObject currentCable;
    public LineRenderer lineRenderer;
    // public float cableLength;
    // public Color cableColor;
    public int segmentCount;
    public float curveAmount;
    public Transform endPoint;
    public Transform startPoint;
    public Vector3[] controlPoints;
    public int count;
    public bool isfixed;

    // Constructor to initialize the cable
    public Cable(string name, Transform startpoint)
    {
        currentCable = new GameObject(name);
        lineRenderer = currentCable.AddComponent<LineRenderer>();

        segmentCount = 200;
        curveAmount = 0.1f;
        controlPoints = new Vector3[segmentCount + 2];
        lineRenderer.positionCount = segmentCount + 2; 
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        count = 0;
        isfixed = false;
        startPoint = startpoint;
    }
}

