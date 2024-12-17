using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public float radius = 200f; // ��ת�뾶
    public float period = 50f; // ��ת���ڣ���λΪ��

    private float angle = 0f;

    void Update()
    {
        // ���㵱ǰʱ��ĽǶ�
        angle += (360f / period) * Time.deltaTime;
        if (angle >= 360f)
        {
            angle -= 360f;
        }

        // ���Ƕ�ת��Ϊ����
        float radian = angle * Mathf.Deg2Rad;

        // �����µ�λ�ã�ȷ�� z ����ʼ��Ϊ 400
        float x = Mathf.Cos(radian) * radius;
        float y = Mathf.Sin(radian) * radius;
        float z = 400f;

        // ���µ��Դ��λ��
        transform.position = new Vector3(x, y, z);
    }
}
