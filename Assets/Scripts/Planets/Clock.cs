using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Clock : MonoBehaviour
{
    // Start is called before the first frame update
    public const float startDay = 0f;
    

    public Text time_text;
    public static float dayTime = 0;
    public static float speed = 0.0f;

    private static float previousSpeed = 1.0f;

    public Button x1Button;
    public Button x2Button;
    public Button x5Button;
    public Button x10Button;
    public Button pauseButton;

    private Button[] buttons;

    void Awake()
    {
        
    }

    void Start()
    {
        //Debug.Log("23333");
        if (!StageController.stageStart)
        { 
            dayTime = startDay;
        }
        buttons = new Button[] { x1Button, x2Button, x5Button, x10Button, pauseButton };
        x1Button.onClick.AddListener(() => OnButtonClick(x1Button, 1.0f));
        x2Button.onClick.AddListener(() => OnButtonClick(x2Button, 2.0f));
        x5Button.onClick.AddListener(() => OnButtonClick(x5Button, 5.0f));
        x10Button.onClick.AddListener(() => OnButtonClick(x10Button, 10.0f));
        pauseButton.onClick.AddListener(() => OnButtonClick(pauseButton, 0.0f));
        OnButtonClick(pauseButton, 0.0f);
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

        //just for test
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            StageController.NextStage(2);
            StageController.LoadStage();
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