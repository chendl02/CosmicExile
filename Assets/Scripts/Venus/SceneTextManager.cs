using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SceneTextManager : MonoBehaviour
{
    public bool isTextOnlyMode;
    public bool modeChange;
    public Text taskText;
    public Text subTaskText;

    void Awake()
    { // 查找场景中的所有EventSystem对象
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        Debug.Log("EventSystem Number: " + eventSystems.Length); // 如果找到多个EventSystem对象，删除多余的
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
            // 禁用其他组件的功能
            DisableOtherComponents();
        }
        else if (!isTextOnlyMode && modeChange)
        {
            // 启用其他组件的功能
            EnableOtherComponents();
        }
    }

    void DisableOtherComponents()
    {
        // 获取场景中的所有组件并禁用它们
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
        // 获取场景中的所有组件并启用它们
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

