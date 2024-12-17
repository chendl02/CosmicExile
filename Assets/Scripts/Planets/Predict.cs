using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Predict : MonoBehaviour
{
    public Toggle toggle;
    public Slider slider;
    public GameObject sliderObject;

    public static int initPredictDays = 100;

    public const int dayLimit = 1000;
    public const float deltaTime = Universe.physicsTimeStep * Universe.timeCoefficient;



    public Color lineColor;

    private LineRendererHandler lineHandler;

    private Queue<Vector3> trajectory;

    private int day;
    private int hour;
    private float dayTimeBegin;
    private float dayTimeEnd;
    private MotionData motionDataEnd;

    // Start is called before the first frame update
    void Start()
    {
        toggle.isOn = false;
        sliderObject.SetActive(false);

        trajectory = new Queue<Vector3>();

        toggle.onValueChanged.AddListener(togglePredict);


        lineHandler = this.gameObject.AddComponent<LineRendererHandler>();
        lineHandler.Initialize(gameObject, lineColor);
        lineHandler.Enable(false);
    }

    void UpdateTrajectory(MotionData data)
    {
        float dayTimeNow = Clock.dayTime;
        float dayTime = dayTimeNow;
        day = 0;
        hour = 2;
        trajectory.Clear();
        do
        {
            data = NBodySimulation.RK4(data, dayTime, deltaTime);
            dayTime += deltaTime / 3600 / 24;
            if (Mathf.Floor((dayTime - dayTimeNow - day)*24) >= hour)
            {
                hour += 1;
                trajectory.Enqueue(data.Position);
            }
            if (hour >= 24)
            {
                hour = 0;
                day += 1;
            }
        }
        while (dayTime <= dayTimeNow + dayLimit + 1);
        dayTimeBegin = dayTimeNow;
        dayTimeEnd = dayTime;
        motionDataEnd = data;
    }

    public void setRenderer(int days)
    {
        int hours = days * 24;
        lineHandler.SetPositions(trajectory.Take(hours).ToArray());
        NBodySimulation.ActivateVirtualMesh(Clock.dayTime + days);
        NBodySimulation.DrawPredict(days);
    }

    void togglePredict(bool isOn)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (isOn)
        {
            if (Clock.speed != 0)
            {
                Clock clock = FindObjectOfType<Clock>();
                clock.pressPause();
            }

            Ship ship = FindObjectOfType<Ship>();
            UpdateTrajectory(ship.motionData);
            setRenderer(NonLinearSlider.previousValidValue);
            lineHandler.Enable(true);
            sliderObject.SetActive(true);
        }
        else
        {
            lineHandler.Enable(false);
            sliderObject.SetActive(false);
            NBodySimulation.DeactivateVirtualMesh();
            NBodySimulation.DrawOrbit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Clock.speed == 0)
            return;
        if (lineHandler.isEnable() == false)
            return;
        if (Mathf.Floor((dayTimeEnd - dayTimeBegin - day)*24) > hour)
        {
            hour += 1;
            trajectory.Enqueue(motionDataEnd.Position);
            trajectory.Dequeue();
            setRenderer(NonLinearSlider.previousValidValue);
            
        }
        if (hour >= 24)
        {
            hour = 0;
            day += 1;
            NBodySimulation.UpdatePredict(dayTimeEnd);
        }
        motionDataEnd = NBodySimulation.RK4(motionDataEnd, dayTimeEnd, deltaTime);
        dayTimeEnd += deltaTime / 3600 / 24;
    }
}
