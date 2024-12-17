using UnityEngine;

public class MarsVehicleTrans : MonoBehaviour
{
    public GameObject astronaut;   // Astronaut object
    public GameObject truck;       // Truck object
    public Camera astronautCamera; // Astronaut's camera
    public Camera truckCamera;     // Truck's camera

    public float switchDistance = 5.0f; // Switch distance between astronaut and truck

    private bool isControllingTruck = false; 
    private MarsAstronautController astronautController;
    private TruckController truckController;

    private Vector3 astronautOffset; 

    void Start()
    {
        astronautController = astronaut.GetComponent<MarsAstronautController>();
        truckController = truck.GetComponent<TruckController>();
        astronautOffset = astronaut.transform.position - truck.transform.position;
        truckController.enabled = false;
        EnableAstronautControl();
    }

    void Update()
    {
        float distance = Vector3.Distance(astronaut.transform.position, truck.transform.position);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isControllingTruck)
            {
                EnableAstronautControl();
            }
            else if (distance <= switchDistance)
            {
                EnableTruckControl();
            }
        }

        if (isControllingTruck && !astronaut.activeSelf)
        {
            astronaut.transform.position = truck.transform.position + astronautOffset;
        }
    }

    void EnableAstronautControl()
    {
        isControllingTruck = false;

        astronaut.SetActive(true);
        
        astronautController.enabled = true;
        truckController.enabled = false;

        astronautCamera.enabled = true;
        truckCamera.enabled = false;
    }

    void EnableTruckControl()
    {
        isControllingTruck = true;
        astronautOffset = astronaut.transform.position - truck.transform.position;
        astronautController.isCarryingU = false;
        astronautController.enabled = false;
        truckController.enabled = true;

        astronaut.SetActive(false);
        astronautCamera.enabled = false;
        truckCamera.enabled = true;
    }
}
