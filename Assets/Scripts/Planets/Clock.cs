using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    // Start is called before the first frame update
    public Text time_text;
    public static float time;
    public static float speed = 1.0f;

    public Button x1Button;
    public Button x2Button;
    public Button x5Button;
    public Button x10Button;
    public Button pauseButton;

    private Button[] buttons;

    void Awake()
    {
        Time.fixedDeltaTime = Universe.physicsTimeStep / speed;
        Debug.Log("Setting fixedDeltaTime to: " + Universe.physicsTimeStep / speed);
    }

    void FixedUpdate()
    {
        if (Clock.speed == 0)
            return;
        time += Universe.physicsTimeStep * Universe.timeCoefficient / 3600.0f / 24.0f;
        time_text.text = time.ToString("F0") + " Days";
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
                // ����Ϊ����״̬��ɫ
                colors.normalColor = colors.pressedColor;
            }
            else
            {
                // ����Ϊ����״̬��ɫ
                colors.normalColor = Color.white; // ����Ĭ����ɫΪ��ɫ
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
        OnButtonClick(x1Button, 1.0f);
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
    }

}
