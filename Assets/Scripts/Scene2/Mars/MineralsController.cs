using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralsController : MonoBehaviour
{
    public MineralsUI mineralsUI; 
    private MarsAstronautController astronautController;
    public string mineralType;   
    public Vector3 labelOffset = new Vector3(0, 0.1f, 0);
    void Start()
    {
        mineralsUI = FindObjectOfType<MineralsUI>();
        astronautController = FindObjectOfType<MarsAstronautController>();
        
        if (mineralsUI == null)
        {
            Debug.LogError("MineralsUI not found in the scene!");
        }
        mineralType = gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string mineralType = gameObject.tag;
            Debug.Log(mineralType + " collected!");

            if (mineralsUI != null)
            {
                switch (mineralType)
                {
                    case "Iron":
                        mineralsUI.AddMinerals(1);
                        break;
                    case "Uranium":
                        mineralsUI.AddMinerals(5);
                        // MarsAstronautController astronautController = other.GetComponent<MarsAstronautController>();
                        // MarsAstronautController astronautController = other.GetComponent<MarsAstronautController>();
                        astronautController.isCarryingU = true;
                        // HealthManager healthManager = FindObjectOfType<HealthManager>();
                        // if (healthManager != null)
                        // {
                        //     healthManager.ReduceHealth(20); 
                        // }
                        // else
                        // {
                        //     Debug.LogWarning("No HealthManager found!");
                        // }
                        break;
                    default:
                        Debug.LogWarning("Unknown mineral type: " + mineralType);
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
    void OnGUI()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main camera not found in the scene!");
            return;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + labelOffset);

        if (screenPosition.z > 0)
        {
            Vector3 direction = (transform.position - Camera.main.transform.position).normalized;
            Ray ray = new Ray(Camera.main.transform.position, direction);
            // Vector3 direction = (transform.position - Camera.main.transform.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, direction, out hit))
            {
                if (hit.transform == transform) 
                {
                    GUI.Label(new Rect(screenPosition.x - 50, Screen.height - screenPosition.y - 50, 100, 30), mineralType);
                }
            }
        }
    }


}