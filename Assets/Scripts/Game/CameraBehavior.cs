using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CameraBehavior : MonoBehaviour
{
    public const float zoomSpeed = 1f; // 缩放速度
    public const float moveSpeed = 1f; // WSAD移动速度
    public const float minZoom = 1f;   // 缩放的最小值
    public const float maxZoom = 5000f;  // 缩放的最大值


    private const float hideCoefficient = 50f;
    private const float showInner = 1500f;


    Camera mapCam;
    GameObject mapCamObject;


    public List<GameObject> labelList, labelList2;

    public Font textFont;

    // 构造函数，接收一个 position 参数，用于初始化摄像机
    public void Initialize(Transform cameraBase)
    {
        mapCamObject = this.transform.gameObject;
        
        Debug.Log(cameraBase.position);

        mapCam = mapCamObject.AddComponent<Camera>();

        // 设置摄像机的位置
        mapCam.transform.position = cameraBase.position + new Vector3(0, 0, -2500);

        // 设置摄像机的旋转方向，使其视角为 z 轴向下
        mapCam.transform.rotation = Quaternion.Euler(0, 0, 0);

        // 设置摄像机参数
        mapCam.farClipPlane = 10000f;
        mapCam.nearClipPlane = 0.1f;
        mapCam.orthographic = true;
        mapCam.orthographicSize = 50;

        // 设置摄像机的标签为 MainCamera
        mapCam.tag = "MainCamera";


        this.transform.SetParent(cameraBase);

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

        labelList2 = new List<GameObject>();

        GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");

        foreach (GameObject ship in ships)
        {
            GameObject textObject = new GameObject(ship.name + "_Label");
            textObject.transform.SetParent(GameObject.Find("UI").transform);

            // 添加Text组件
            Text textComponent = textObject.AddComponent<Text>();
            textComponent.text = "Ship";
            textComponent.fontSize = 24;
            textFont = Resources.Load<Font>("Consolas");
            textComponent.font = textFont;
            textComponent.color = Color.green;
            textComponent.alignment = TextAnchor.MiddleCenter;
            Outline outline = textComponent.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.white;

            // 设置Text对象的位置
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(ship.transform.position);

            labelList2.Add(textObject);

        }

    }
    void Start()
    {
        
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

                LineRendererHandler.setWidthMap(camera.orthographicSize);


                
            }

            // WSAD�ƶ�
            float horizontal = Input.GetAxis("Horizontal"); // A/D �� ��/�ҷ����
            float vertical = Input.GetAxis("Vertical");     // W/S �� ��/�·����

            if (horizontal != 0.0f || vertical != 0.0f)
            {
                Vector3 move = new Vector3(horizontal, vertical, 0) * moveSpeed * camera.orthographicSize * Time.deltaTime;
                camera.transform.Translate(move, Space.World);
            }


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


            foreach (GameObject child in labelList2)
            {
                //Debug.Log("update");
                GameObject ship = GameObject.Find(child.name.Replace("_Label", ""));
                if (ship != null)
                {
                    RectTransform rectTransform = child.GetComponent<RectTransform>();
                    Vector3 vector = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(ship.transform.position) - vector;
                }

                child.SetActive(camera.orthographicSize > 5);

            }

        }
    }

    void OnDestroy()
    {
        foreach (GameObject child in labelList)
        {
            Destroy(child);
        }

        foreach (GameObject child in labelList2)
        {
            Destroy(child);
        }

        LineRendererHandler.setWidthDefault();
    }
}
