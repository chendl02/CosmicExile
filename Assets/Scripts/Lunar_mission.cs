using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskHandler : MonoBehaviour
{
    public VehicleSwitch vehicleSwitch; // 引用角色切换脚本
    public Transform targetC;           // 目标点 C
    public float interactionDistance = 40.0f; // 交互距离

    private bool mission_completed = false;

    public TextMeshProUGUI mission_update;

    public TextMeshProUGUI mission_description;

    private bool showdescription = false;

    void Update()
    {
        // 获取当前控制角色的状态
        bool isControllingTruck = vehicleSwitch.IsControllingTruck();

        // Vector3 currentCharacter = vehicleSwitch.getTruckPosition();
        // // if(isControllingTruck){
        // //     Debug.Log("is controlling truck");
        // //     Debug.Log(vehicleSwitch.getTruckPosition());
        // // }
        // if(!isControllingTruck){
        //     //Debug.Log("is controlling as");
        //     //Debug.Log(vehicleSwitch.getAstronautPosition());
        //     //Debug.Log( Vector3.Distance(vehicleSwitch.getAstronautPosition(), targetC.position));
        //     currentCharacter = vehicleSwitch.getAstronautPosition();
        // }
        Vector3 currentCharacter = isControllingTruck ? vehicleSwitch.getTruckPosition(): vehicleSwitch.getAstronautPosition();

        // 检测当前角色与目标 C 的距离
        float distanceToC = Vector3.Distance(currentCharacter, targetC.position);

        // 如果当前角色是 Astronaut 且距离小于交互距离，允许完成任务

        Debug.Log(distanceToC);
        Debug.Log(isControllingTruck);
        if (!isControllingTruck && distanceToC <= interactionDistance && !mission_completed)
        {
            if (Input.GetKeyDown(KeyCode.I)) // 按下 I 键
            {
                CompleteTask();
            }
        }
        else if (isControllingTruck && distanceToC <= interactionDistance && !mission_completed)
        {
            // Truck 无法完成任务，提示
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("You have to get off from truck to get water.");
                mission_update.gameObject.SetActive(true);
                mission_update.text = "You have to get off from truck to get water.";

            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) // 按下 I 键
        {
            if(showdescription){
                mission_description.gameObject.SetActive(false);
                showdescription = false;
            }
            else{
                mission_description.gameObject.SetActive(true);
                mission_description.text = "You need to collect water to continue your interstellar journey. The moon contains a small amount of ice, and you must follow the direction indicated by the blue beacon to locate the ice. Press the I key to collect the ice. Once the collection is successful, you need to follow the direction indicated by the orange beacon to return to the rocket. Moving too far away from the terminator line will expose you to extreme temperature damage. Therefore, try to stay close to the terminator line or remain inside the truck for protection.";
                showdescription = true;
            }
        }
    }

    void CompleteTask()
    {
        Debug.Log("Successfully get water!");
        mission_update.gameObject.SetActive(true);
        mission_update.text = "Successfully get water! Go back to rocket.";

        mission_completed = true;
        // 在这里实现任务完成后的逻辑，例如：
        // - 更改任务状态
        // - 激活下一阶段目标
        // - 播放音效或动画
    }
}