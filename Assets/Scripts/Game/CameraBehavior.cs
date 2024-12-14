using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float zoomSpeed = 1f; // ���������ٶ�
    public float moveSpeed = 1f; // WSAD�ƶ��ٶ�
    public float minZoom = 5f;    // ���ŵ���Сֵ
    public float maxZoom = 50f;   // ���ŵ����ֵ
    void Start()
    {
        
    }

    // Update is called once per frame
    public void set_zoom()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("no main camera");
        }
        if (camera != null)
        {
            minZoom = camera.orthographicSize / 20;
            maxZoom = camera.orthographicSize * 100;
        }
    }
    void Update()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("no main camera");
        }
        if (camera != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                camera.orthographicSize -= scroll * zoomSpeed * camera.orthographicSize; // ������������
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);

                //Debug.Log(camera.orthographicSize);

                // �����͸�����
                // camera.fieldOfView -= scroll * zoomSpeed;
                // camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minZoom, maxZoom);
            }

            // WSAD�ƶ�
            float horizontal = Input.GetAxis("Horizontal"); // A/D �� ��/�ҷ����
            float vertical = Input.GetAxis("Vertical");     // W/S �� ��/�·����

            Vector3 move = new Vector3(horizontal, vertical, 0) * moveSpeed * camera.orthographicSize * Time.deltaTime;
            camera.transform.Translate(move, Space.World);
        }
    }
}
