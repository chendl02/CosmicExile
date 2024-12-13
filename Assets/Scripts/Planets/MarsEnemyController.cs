using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsEnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log($"Player collided with {gameObject.name}");

            HealthManager healthManager = FindObjectOfType<HealthManager>();
            if (healthManager != null)
            {
                healthManager.ReduceHealth(20); 
            }
            else
            {
                Debug.LogWarning("No HealthManager found!");
            }

            
            Destroy(gameObject);
        }
    }
}
