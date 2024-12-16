using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CableCollision : MonoBehaviour
{
    public AudioClip audioClip;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Astronaut")
        {
            OnPlayerTouchCable();
        }
    }

    void OnPlayerTouchCable()
    {
        TaskController taskController = GameObject.Find("SceneManager").GetComponent<TaskController>();
        if (taskController == null)
        {
            Debug.Log("TaskController component not found on Canvas!");
            return;
        }
        taskController.enabled = true;

        if (taskController.currentCableNum < 10)
        {
            audioClip = Resources.Load<AudioClip>("cable");
            GameObject audioObject = new GameObject("AudioObject");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.Play();
            Destroy(audioObject, audioClip.length);
            taskController.currentCableNum += 1;
            taskController.AddMessage("<color=Green>Message</color>: Collect One Cable!!");
            Destroy(gameObject);
        }
        else
        {
            taskController.AddMessage("<color=orange>Warning</color>: Out of Capacity!!");
        }
    }

}
