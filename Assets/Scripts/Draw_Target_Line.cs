using UnityEngine;

public class DynamicTargetLine : MonoBehaviour {
    public Transform target; // 目标物体

    public Transform start; // Astronaut

    public Transform planet;  // 另一个物体（Planet）

    //public Transform rocket; // Rocket

    public Camera mainCamera; // 摄像机

    private LineRenderer lineRenderer; // LineRenderer 组件


    public VehicleSwitch vehicleSwitch; // 引用角色切换脚本

    void Start() {
        // 获取 LineRenderer 组件
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // 确保摄像机不为空
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }
    }

    void Update() {
        // if (currentCharacter == null) {
        //     return;
        // }

        bool isControllingTruck = vehicleSwitch.IsControllingTruck();

        if(!isControllingTruck){
            RenderLine();
        }
        else{
            lineRenderer.enabled = false;
        }



        // 如果目标物体存在
        // if (target != null && planet != null && mainCamera != null && start!=null) {
        //     // 设置线条的起点和终点
        //     // lineRenderer.SetPosition(0, start.position); // 起点：摄像机位置
        //     // lineRenderer.SetPosition(1, target.position); // 终点：目标物体位置
        //     Vector3 startToPlanet = (planet.position - start.transform.position).normalized;

        //     Vector3 startToTarget = (target.position - start.position).normalized;

        //     Vector3 planeNormal = Vector3.Cross(startToPlanet, startToTarget).normalized;

        //     Vector3 perpendicularDirection = Vector3.Cross(planeNormal, startToPlanet).normalized * 2;

        //     Vector3 lineStart = start.position - startToPlanet*2; // 线段起点是摄像机位置
        //     Vector3 lineEnd = lineStart + perpendicularDirection ; // 线段终点，长度为 1

        //     lineRenderer.SetPosition(0, lineStart);
        //     lineRenderer.SetPosition(1, lineEnd);

        // }
    }

    public void RenderLine(){
        if (target != null && planet != null && mainCamera != null && start!=null) {
            lineRenderer.enabled = true;
            // 设置线条的起点和终点
            // lineRenderer.SetPosition(0, start.position); // 起点：摄像机位置
            // lineRenderer.SetPosition(1, target.position); // 终点：目标物体位置
            Vector3 startToPlanet = (planet.position - start.transform.position).normalized;

            Vector3 startToTarget = (target.position - start.position).normalized;

            Vector3 planeNormal = Vector3.Cross(startToPlanet, startToTarget).normalized;

            Vector3 perpendicularDirection = Vector3.Cross(planeNormal, startToPlanet).normalized * 2;

            Vector3 lineStart = start.position - startToPlanet*2; // 线段起点是摄像机位置
            Vector3 lineEnd = lineStart + perpendicularDirection ; // 线段终点，长度为 1

            lineRenderer.SetPosition(0, lineStart);
            lineRenderer.SetPosition(1, lineEnd);

        }
    }
}
