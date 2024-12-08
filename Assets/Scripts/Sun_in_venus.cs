using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public float radius = 200f; // 旋转半径
    public float period = 50f; // 旋转周期，单位为秒

    private float angle = 0f;

    void Update()
    {
        // 计算当前时间的角度
        angle += (360f / period) * Time.deltaTime;
        if (angle >= 360f)
        {
            angle -= 360f;
        }

        // 将角度转换为弧度
        float radian = angle * Mathf.Deg2Rad;

        // 计算新的位置，确保 z 坐标始终为 400
        float x = Mathf.Cos(radian) * radius;
        float y = Mathf.Sin(radian) * radius;
        float z = 400f;

        // 更新点光源的位置
        transform.position = new Vector3(x, y, z);
    }
}
