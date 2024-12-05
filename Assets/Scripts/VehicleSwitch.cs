using UnityEngine;

public class VehicleSwitch : MonoBehaviour
{
    public GameObject astronaut;  // Astronaut 对象
    public GameObject truck;      // Truck 对象
    public Camera astronautCamera; // Astronaut 的摄像机
    public Camera truckCamera;     // Truck 的摄像机

    public float switchDistance = 5.0f; // Astronaut 和 Truck 之间的切换距离

    private bool isControllingTruck = false; // 当前是否控制 Truck
    private AstronautController astronautController;
    private TruckController truckController;

    private Vector3 astronautOffset; // Astronaut 与 Truck 的相对位置

    void Start()
    {
        // 获取角色的控制脚本
        astronautController = astronaut.GetComponent<AstronautController>();
        truckController = truck.GetComponent<TruckController>();
        
        astronautOffset = astronaut.transform.position - truck.transform.position;

        // 确保一开始控制 Astronaut
        EnableAstronautControl();
    }

    void Update()
    {
        float distance = Vector3.Distance(astronaut.transform.position, truck.transform.position);

        // 检测按键并切换控制
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isControllingTruck)
            {
                EnableAstronautControl();
            }
            else if(distance <= switchDistance)
            {
                EnableTruckControl();
            }
        }

        // 操控 Truck 时让隐藏的 Astronaut 跟随
        if (isControllingTruck && !astronaut.activeSelf)
        {
            astronaut.transform.position = truck.transform.position + astronautOffset;
        }
    }

    void EnableAstronautControl()
    {
        isControllingTruck = false;

        // 显示 Astronaut
        astronaut.SetActive(true);

        // 启用 Astronaut 控制
        astronautController.enabled = true;
        truckController.enabled = false;

        // 启用 Astronaut 摄像机
        astronautCamera.enabled = true;
        truckCamera.enabled = false;

        
    }

    void EnableTruckControl()
    {
        isControllingTruck = true;

        astronautOffset = astronaut.transform.position - truck.transform.position;

        

        // 启用 Truck 控制
        astronautController.enabled = false;
        truckController.enabled = true;

        // 隐藏 Astronaut
        astronaut.SetActive(false);

        // 启用 Truck 摄像机
        astronautCamera.enabled = false;
        truckCamera.enabled = true;
    }
}

