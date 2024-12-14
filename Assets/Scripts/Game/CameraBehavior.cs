using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CameraBehavior : MonoBehaviour
{
    public float zoomSpeed = 1f; // 缩放速度
    public float moveSpeed = 1f; // WSAD移动速度
    public float minZoom = 5f;   // 缩放的最小值
    public float maxZoom = 50f;  // 缩放的最大值


    private float hideCoefficient = 50f;
    private float showInner = 1500f;


    Camera mapCam;
    GameObject mapCamObject;


    public List<GameObject> labelList;

    public Font textFont;

    // 构造函数，接收一个 position 参数，用于初始化摄像机
    public void Initialize(Vector3 position)
    {
        mapCamObject = this.transform.gameObject;
        mapCam = mapCamObject.AddComponent<Camera>();

        // 设置摄像机的位置
        mapCam.transform.position = position + new Vector3(0, 0, -2500);

        // 设置摄像机的旋转方向，使其视角为 z 轴向下
        mapCam.transform.rotation = Quaternion.Euler(0, 0, 0);

        // 设置摄像机参数
        mapCam.farClipPlane = 10000f;
        mapCam.nearClipPlane = 0.1f;
        mapCam.orthographic = true;
        mapCam.orthographicSize = 50;

        // 设置摄像机的标签为 MainCamera
        mapCam.tag = "MainCamera";
        set_zoom();

        labelList = new List<GameObject>();

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach (GameObject planet in planets)
        {
            GameObject textObject = new GameObject(planet.name + "_Label");
            textObject.transform.SetParent(GameObject.Find("UI").transform);

            // 添加Text组件
            Text textComponent = textObject.AddComponent<Text>();
            textComponent.text = planet.name;
            textComponent.fontSize = 24;
            textFont = Resources.Load<Font>("Consolas");
            textComponent.font = textFont;
            textComponent.color = Color.white;
            textComponent.alignment = TextAnchor.MiddleCenter;
            Outline outline = textComponent.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.white;

            // 设置Text对象的位置
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(planet.transform.position);

            labelList.Add(textObject);

        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void set_zoom()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("no main camera");
        }
        if (camera != null)
        {
            minZoom = camera.orthographicSize / 10;
            maxZoom = camera.orthographicSize * 100;
        }
    }
    void Update()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("no main camera");
        }
        if (camera != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                camera.orthographicSize -= scroll * zoomSpeed * camera.orthographicSize; // ������������
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);

                //Debug.Log(camera.orthographicSize);

                // �����͸�����
                // camera.fieldOfView -= scroll * zoomSpeed;
                // camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minZoom, maxZoom);
            }

            // WSAD�ƶ�
            float horizontal = Input.GetAxis("Horizontal"); // A/D �� ��/�ҷ����
            float vertical = Input.GetAxis("Vertical");     // W/S �� ��/�·����

            Vector3 move = new Vector3(horizontal, vertical, 0) * moveSpeed * camera.orthographicSize * Time.deltaTime;
            camera.transform.Translate(move, Space.World);



            // Label
            //foreach (Text child in labels.GetComponentsInChildren<Text>())
            foreach (GameObject child in labelList)
            {
                //Debug.Log("update");
                GameObject planet = GameObject.Find(child.name.Replace("_Label", ""));
                if (planet != null)
                {
                    RectTransform rectTransform = child.GetComponent<RectTransform>();
                    Vector3 vector = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(planet.transform.position) - vector;
                }

                CelestialBody body = planet.GetComponent<CelestialBody>();

                if (body.inner)
                {
                    child.SetActive(camera.orthographicSize <= showInner && camera.orthographicSize >= body.radius * hideCoefficient);
                }
                else
                {
                    child.SetActive(camera.orthographicSize >= body.radius * hideCoefficient);
                }

                
            }
        }
    }

    void OnDestroy()
    {
        foreach (GameObject child in labelList)
        {
            Destroy(child);
        }
    }
}
