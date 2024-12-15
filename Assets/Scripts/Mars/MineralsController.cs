using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralsController : MonoBehaviour
{
    public MineralsUI mineralsUI; 
    private MarsAstronautController astronautController;

    void Start()
    {
        mineralsUI = FindObjectOfType<MineralsUI>();
        astronautController = FindObjectOfType<MarsAstronautController>();
        
        if (mineralsUI == null)
        {
            Debug.LogError("MineralsUI not found in the scene!");
        }
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
}