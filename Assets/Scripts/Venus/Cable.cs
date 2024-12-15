using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CableCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Astronaut")
        {
            OnPlayerTouchCable();
        }
    }

    void OnPlayerTouchCable()
    {
        TaskController taskController = GameObject.Find("Canvas").GetComponent<TaskController>();
        if (taskController.currentCableNum < 10)
        {
            taskController.currentCableNum += 1;
            taskController.AddMessage("<color=Green>Message</color>: Collect One Cable!!");
            Destroy(gameObject);
        }
        else {
            taskController.AddMessage("<color=orange>Warning</color>: Out of Capasity!!");
        }
    }
}
