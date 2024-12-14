using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    // Start is called before the first frame update
    public Text time_text;
    public static float dayTime;
    public static float speed = 1.0f;

    private static float previousSpeed = 1.0f;

    public Button x1Button;
    public Button x2Button;
    public Button x5Button;
    public Button x10Button;
    public Button pauseButton;

    private Button[] buttons;

    public GameObject targetObject; 
    public Font textFont;
    public Vector3 canvasInitialPosition;
    public List<Text> labelList;

    void Awake()
    {
        Time.fixedDeltaTime = Universe.physicsTimeStep / speed;
        Debug.Log("Setting fixedDeltaTime to: " + Universe.physicsTimeStep / speed);
        canvasInitialPosition = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        if (Clock.speed == 0)
            return;
        dayTime += Universe.physicsTimeStep * Universe.timeCoefficient / 3600.0f / 24.0f;
        time_text.text = dayTime.ToString("F0") + " Days";
    }

    void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        if (speed != 0)
        {
            Time.fixedDeltaTime = Universe.physicsTimeStep / speed;
            Debug.Log("Setting fixedDeltaTime to: " + Universe.physicsTimeStep / speed);
        }
    }

    void SetButtonState(Button clickedButton)
    {
        foreach (var button in buttons)
        {
            var colors = button.colors;
            if (button == clickedButton)
            {
                colors.normalColor = colors.pressedColor;
            }
            else
            {
                colors.normalColor = Color.white;
            }
            button.colors = colors;
        }
    }
    private void OnButtonClick(Button clickedButton, float newSpeed)
    {
        SetButtonState(clickedButton);
        SetSpeed(newSpeed);
    }
        void Start()
    {
        buttons = new Button[] { x1Button, x2Button, x5Button, x10Button, pauseButton };
        x1Button.onClick.AddListener(() => OnButtonClick(x1Button, 1.0f));
        x2Button.onClick.AddListener(() => OnButtonClick(x2Button, 2.0f));
        x5Button.onClick.AddListener(() => OnButtonClick(x5Button, 5.0f));
        x10Button.onClick.AddListener(() => OnButtonClick(x10Button, 10.0f));
        pauseButton.onClick.AddListener(() => OnButtonClick(pauseButton, 0.0f));
        OnButtonClick(pauseButton, 0.0f);
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        //GameObject motion = GameObject.Find("motion");
        labelList = new List<Text>();
        foreach (GameObject planet in planets)
        {
            GameObject textObject = new GameObject(planet.name + "_Label");
            textObject.transform.SetParent(gameObject.transform);

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
            labelList.Add(textComponent);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnButtonClick(x1Button, 1.0f);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnButtonClick(x2Button, 2.0f);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            OnButtonClick(x5Button, 5.0f);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnButtonClick(x10Button, 10.0f);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnButtonClick(pauseButton, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressPause();
        }
        Camera currentCamera = Camera.main;
        if (currentCamera != null && currentCamera.name == "Ship Camera")
        {
            foreach (Text child in labelList)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Text child in labelList)
            {
                GameObject planet = GameObject.Find(child.name.Replace("_Label", ""));
                child.gameObject.SetActive(true);
                if (planet != null)
                {
                    RectTransform rectTransform = child.GetComponent<RectTransform>();
                    Vector3 vector = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(planet.transform.position) - vector;
                }
            }
        }
    }

    public void pressPause()
    {
        if (speed == 0.0f)
        {
            SetSpeed(previousSpeed);
        }
        else
        {
            previousSpeed = speed;
            SetSpeed(0.0f);
        }
    }

}