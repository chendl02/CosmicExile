using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MarsBackToSpace : MonoBehaviour
{
    public Transform player;          
    public Vector3 targetPosition;     
    public float triggerDistance = 3.0f; 
    public string targetSceneName;     
    
    public MineralsUI mineralsUI; 
    private bool isPlayerNear = false; 
    private bool showUI = false;       
    private bool isTaskDone = false;       
    void start()
    {
        // mineralsUI = FindObjectOfType<MineralsUI>();
        if (mineralsUI == null)
        {
            Debug.LogError("MineralsUI not found in the scene!");
        }
    }
    
    void Update()
    {
        GameObject launchingPad = GameObject.Find("Launching Pad01"); 
        if (launchingPad != null) { targetPosition = launchingPad.transform.position; Debug.Log("Launching Pad01 Position: " + targetPosition); }
        float distance = Vector3.Distance(player.position, targetPosition);
        isTaskDone = mineralsUI.isDone();
        if (distance <= triggerDistance )
        {
            isPlayerNear = true;

            if (Input.GetKeyDown(KeyCode.F) && isTaskDone)
            {
                SwitchScene();
            }
        }
        else
        {
            isPlayerNear = false;
        }
    }

    void OnGUI()
    {
        if (isPlayerNear)
        {   
            if(isTaskDone)
            {
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 40), "Press F to go back to" + targetSceneName);
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 40), "Need more minerals to upgrade the spaceship!");
            }
        }
        
    }

    void SwitchScene()
    {
        StageController.NextStage(4);
        SceneManager.LoadScene(targetSceneName);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetPosition, triggerDistance);
    }
}
