using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

// public enum TaskState { 
//     NotStarted, 
//     Task1,
//     Task2,
//     Task3,
//     Task4,
//     Completed, 
// }

public class MarsTaskController : MonoBehaviour
{
    public Text taskText;
    public Text subTaskText;
    public Text objectText;
    public Text messageText;
    public TaskState currentState;
    public MarsSceneTextManager sceneManager;
    public MarsAstronautController Player;
    public Scrollbar MineralsProgressBar; 
    public string tempText;
    public int currentCableNum;
    // public List<ThunderArea> thunderAreaList = new List<ThunderArea>();
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
        Player = GameObject.Find("Astronaut").GetComponent<MarsAstronautController>();
        subTaskText.text = "";
        sceneManager = GameObject.Find("SceneManager").GetComponent<MarsSceneTextManager>();
        if (taskText == null)
        { Debug.LogError("TaskText component not found!"); }
        else
        {
            currentState = TaskState.NotStarted;
            string startText = "Welcome to <color=red>Mars</color>! \nYour task is to collect Ore materials, <color=red>Iron and Uranium</color>. Beware of uranium radiation that can harm you. \n Try to avoid the aliens on the planet! \n Also, be careful not to get caught in a sandstorm. \nIf you are ready, press <color=red>'Enter'</color> to start game!";
            StartCoroutine(ShowText(taskText, startText, false));
        }
        // GameObject[] thunderAreas = GameObject.FindGameObjectsWithTag("ThunderArea");
        finishTask = false;
        // foreach (GameObject thunderArea in thunderAreas)
        // {
        //     thunderAreaList.Add(thunderArea.GetComponent<ThunderArea>());
        // }
        panelActive = false;
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
                yield return new WaitForSeconds(5f); 
                messageText.text = ""; 
            }
            else
            {
                yield return null; 
            }
        }
    }
    IEnumerator ShowText(Text textObject, string fullText, bool changeModeAfterText)
    {
        sceneManager.isTextOnlyMode = true;
        sceneManager.modeChange = true;
        textObject.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            textObject.text += fullText[i];
            yield return new WaitForSeconds(0.04f); 
        }


        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; 
        } 
        OnEnterPressed(currentState);

            for (float alpha = 1f; alpha >= 0; alpha -= 0.2f)
        {
            Color color = taskText.color;
            color.a = alpha;
            textObject.color = color;
            yield return new WaitForSeconds(0.1f); 
        }
        sceneManager.isTextOnlyMode = false;
        sceneManager.modeChange = changeModeAfterText;
        textObject.text = "";
        currentState = TaskState.Task1;
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
        // objectText.text = "Max Cable Capacity: <color=red>10</color>\n\nCable Frame Number:<color=red>" + currentCableNum + "</color>";
        // if (currentState == TaskState.Task1)
        // {
        subTaskText.text = "Beware of the aliens that are attacking each other, if you get too close they will see you as a common enemy and attack you at a faster rate. \n And the aliens that are wandering around will notice you from further away.";
        //     if (Player.speed > 50f) { ChangeState(currentState); }
        // }
        // if (currentState == TaskState.Task2)
        // {
        //     subTaskText.text = "Good Job!!!, Start the SubTask\nEnter the Twin Planet of Venus\n";
        //     Transform TwinInnerTransform = GameObject.Find("TwinPlanet").transform;
        //     float distanceToTwin = Vector3.Distance(TwinInnerTransform.position, Player.position);
        //     if (distanceToTwin < 1000f) { ChangeState(currentState); }
        // }
        // if (currentState == TaskState.Task3)
        // {
        //     subTaskText.text = "Good Job!!!, Start the SubTask\nCollect 10 Cable Fragments on the Twin Planet\n";
        //     if (currentCableNum == 10) { ChangeState(currentState); }
        // }
        // if (currentState == TaskState.Task4)
        // {
        //     subTaskText.text = "Good Job!!!, You Finish All Begining Task!\nNow Go Back To Venus to Fight Against <color=red>Aliens</color>.\nAnd Place Cable to All the <color=red>Thunder Area</color>.";
        //     foreach (ThunderArea thunderArea in thunderAreaList)
        //     {
        //         if (thunderArea.connected == false) { return; }
        //     }
        //     finishTask = true;
        //     ChangeState(currentState);
        // }
        if (MineralsProgressBar.size == 20)
        {
            subTaskText.text = "Conguadulations!!!, You Finished The Task!!!\nNow Spaceship is more powerful now!!!\n Go Back To the <color=red>SpaceShip</color>.\nAnd You Can Go Back to The Solar System.";
            // foreach (ThunderArea thunderArea in thunderAreaList)
            // {
            //     if (thunderArea.connected == false) { return; }
            // }
            finishTask = true;
            // ChangeState(currentState);
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



