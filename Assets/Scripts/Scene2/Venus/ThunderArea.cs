using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderArea : MonoBehaviour
{
    public AstronautControllerVenus Player;
    public bool connected;
    void Start()
    {
        Player = GameObject.Find("Astronaut").GetComponent<AstronautControllerVenus>();
        connected = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Astronaut")
        {
            OnPlayerEnterThunderArea();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Astronaut")
        {
            Player.enterThunderArea = false; 
            Player.currentThunderArea = null;
        }
    }

    void OnPlayerEnterThunderArea()
    {
        TaskController taskController = GameObject.Find("SceneManager").GetComponent<TaskController>();
        taskController.AddMessage("<color=Yellow>Hint</color>: Enter Thunder Area!! Place Cable Now");
        Player.currentThunderArea = gameObject.GetComponent<ThunderArea>();

        Player.enterThunderArea = true;
    }
}
