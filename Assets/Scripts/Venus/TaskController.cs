using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public enum TaskState { 
    NotStarted, 
    Task1,
    Task2,
    Task3,
    Task4,
    Completed, 
}

public class TaskController : MonoBehaviour
{
    public Text taskText;
    public Text subTaskText;
    public Text objectText;
    public Text messageText;
    public TaskState currentState;
    public SceneTextManager sceneManager;
    public AstronautControllerVenus Player;
    public string tempText;
    public int currentCableNum;
    public List<ThunderArea> thunderAreaList = new List<ThunderArea>();
    private Queue<string> messageQueue = new Queue<string>();
    private bool finishTask;
    public bool panelActive;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    void Start()
    {
        currentCableNum = 0;
        taskText = GameObject.Find("TaskText").GetComponent<Text>();
        subTaskText = GameObject.Find("SubTaskText").GetComponent<Text>();
        objectText = GameObject.Find("ObjectText").GetComponent<Text>();
        messageText = GameObject.Find("MessageText").GetComponent<Text>();
        Player = GameObject.Find("Astronaut").GetComponent<AstronautControllerVenus>();
        subTaskText.text = "";
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneTextManager>();
        if (taskText == null)
        { Debug.LogError("TaskText component not found!"); }
        else
        {
            currentState = TaskState.NotStarted;
            string startText = "Welcome to <color=red>Venus</color>! \nYour task is to collect <color=red>cable</color> materials. \nAnd connect them to the <color=red>thunder</color> areas to charge the spaceship! \nIf you are ready, press <color=red>'Enter'</color> to start game!";
            StartCoroutine(ShowText(taskText, startText, false));
        }
        GameObject[] thunderAreas = GameObject.FindGameObjectsWithTag("ThunderArea");
        finishTask = false;
        foreach (GameObject thunderArea in thunderAreas)
        {
            thunderAreaList.Add(thunderArea.GetComponent<ThunderArea>());
        }
        panelActive = true;
        panel1 = GameObject.Find("Panel1"); 
        panel2 = GameObject.Find("Panel2");
        panel3 = GameObject.Find("Panel3");
    }
    public void AddMessage(string message)
    {
        if (messageQueue.Count >= 4) { messageQueue.Dequeue(); }
        messageQueue.Enqueue(message);
        //StartCoroutine(DisplayMessages());
        messageText.text = string.Join("\n", messageQueue.ToArray());
    }

    IEnumerator DisplayMessages()
    {
        while (true)
        {
            if (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                messageText.text = message;
                yield return new WaitForSeconds(5f); // 显示5秒
                messageText.text = ""; // 清空文字
            }
            else
            {
                yield return null; // 等待下一帧
            }
        }
    }
    IEnumerator ShowText(Text textObject, string fullText, bool changeModeAfterText)
    {
        // 文字缓慢出现
        sceneManager.isTextOnlyMode = true;
        sceneManager.modeChange = true;
        textObject.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            textObject.text += fullText[i];
            yield return new WaitForSeconds(0.04f); // 每个字符出现的间隔时间
        }

        // 展示10秒
        // yield return new WaitForSeconds(10f);

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // 等待下一帧
        } // 玩家按下回车键后触发的逻辑
        OnEnterPressed(currentState);

            // 文字缓慢消失
            for (float alpha = 1f; alpha >= 0; alpha -= 0.2f)
        {
            Color color = taskText.color;
            color.a = alpha;
            textObject.color = color;
            yield return new WaitForSeconds(0.1f); // 每个字符消失的间隔时间
        }
        sceneManager.isTextOnlyMode = false;
        sceneManager.modeChange = changeModeAfterText;
        textObject.text = "";
        currentState = TaskState.Task2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        { 
            panel1.SetActive(!panelActive); 
            panel2.SetActive(!panelActive); 
            panel3.SetActive(!panelActive); 
            panelActive = !panelActive; 
        }
            objectText.text = "Max Cable Capacity: <color=red>10</color>\n\nCable Frame Number:<color=red>" + currentCableNum + "</color>";
        if (currentState == TaskState.Task1)
        {
            subTaskText.text = "Now, Start the SubTask\nFind the Secret of Venus: Use Gravity To Reach 50f Speed\n" + "Current Speed:"+Player.speed;
            if (Player.speed > 50f) { ChangeState(currentState); }
        }
        if (currentState == TaskState.Task2)
        {
            subTaskText.text = "Good Job!!!, Start the SubTask\nEnter the Twin Planet of Venus\n";
            Transform TwinInnerTransform = GameObject.Find("TwinPlanet").transform;
            float distanceToTwin = Vector3.Distance(TwinInnerTransform.position, Player.position);
            if (distanceToTwin < 1000f) { ChangeState(currentState); }
        }
        if (currentState == TaskState.Task3)
        {
            subTaskText.text = "Good Job!!!, Start the SubTask\nCollect 10 Cable Fragments on the Twin Planet\n";
            if (currentCableNum == 10) { ChangeState(currentState); }
        }
        if (currentState == TaskState.Task4)
        {
            subTaskText.text = "Good Job!!!, You Finish All Begining Task!\nNow Go Back To Venus to Fight Against <color=red>Aliens</color>.\nAnd Place Cable to All the <color=red>Thunder Area</color>.";
            foreach (ThunderArea thunderArea in thunderAreaList)
            {
                if (thunderArea.connected == false) { return; }
            }
            finishTask = true;
            ChangeState(currentState);
        }
        if (currentState == TaskState.Completed)
        {
            subTaskText.text = "Conguadulations!!!, You Finish All Task!!!\nNow Spaceship is Full of Energy!!!\n Go Back To the <color=red>SpaceShip</color>.\nAnd You Can Go Back to The Solar System<color=red>Thunder Area</color>.";
            foreach (ThunderArea thunderArea in thunderAreaList)
            {
                if (thunderArea.connected == false) { return; }
            }
            finishTask = true;
            ChangeState(currentState);
        }
    }
    void OnEnterPressed(TaskState State)
    {
        if (State == TaskState.NotStarted)
        {
            sceneManager.isTextOnlyMode = false;
            sceneManager.modeChange = true;
            ChangeState(State);
        }
    }
    void ChangeState(TaskState State)
    {
        subTaskText.text = "";
        sceneManager.isTextOnlyMode = false;
        sceneManager.modeChange = true;
        taskText.text = "";
        currentState = State + 1;
    }

}



