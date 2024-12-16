using UnityEngine;

public class RotateAroundYAxis : MonoBehaviour
{
    public float rotationSpeed = -10f; // 每秒旋转的角速度（度/秒）

    void Update()
    {
        // 绕自身的 Y 轴旋转
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}