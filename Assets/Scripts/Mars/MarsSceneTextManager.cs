using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MarsSceneTextManager : MonoBehaviour
{
    public bool isTextOnlyMode;
    public bool modeChange;
    public Text taskText;
    public Text subTaskText;

    void Awake()
    { 
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        Debug.Log("EventSystem Number: " + eventSystems.Length); 
        for (int i = 1; i < eventSystems.Length; i++)
        {
            Destroy(eventSystems[i].gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        taskText = GameObject.Find("TaskText").GetComponent<Text>();
        subTaskText = GameObject.Find("SubTaskText").GetComponent<Text>();
    }

    void Update()
    {
        if (isTextOnlyMode && modeChange)
        {
            DisableOtherComponents();
        }
        else if (!isTextOnlyMode && modeChange)
        {
            EnableOtherComponents();
        }
    }

    void DisableOtherComponents()
    {
        MonoBehaviour[] allComponents = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour component in allComponents)
        {
            if (component != this && component != taskText && component != subTaskText)
            {
                component.enabled = false;
            }
        }
        modeChange = false;
    }

    void EnableOtherComponents()
    {
        MonoBehaviour[] allComponents = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour component in allComponents)
        {
            if (component != this && component != taskText && component != subTaskText)
            {
                component.enabled = true;
            }
        }
        modeChange = false;
    }
}
