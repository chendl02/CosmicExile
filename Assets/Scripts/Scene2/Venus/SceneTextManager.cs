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
    { // ���ҳ����е�����EventSystem����
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        Debug.Log("EventSystem Number: " + eventSystems.Length); // ����ҵ����EventSystem����ɾ�������
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
            // ������������Ĺ���
            DisableOtherComponents();
        }
        else if (!isTextOnlyMode && modeChange)
        {
            // ������������Ĺ���
            EnableOtherComponents();
        }
    }

    void DisableOtherComponents()
    {
        // ��ȡ�����е������������������
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
        // ��ȡ�����е������������������
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

