using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    public AudioSource audioSource; // ���AudioSource���
    private string microphone;

    void Start()
    {
        // ��ȡ���õ���˷��豸
        if (Microphone.devices.Length > 0)
        {
            microphone = Microphone.devices[0]; // ʹ�õ�һ����˷��豸
            audioSource.clip = Microphone.Start(microphone, true, 10, 44100); // ��ʼ¼��
            audioSource.loop = true; // ����Ϊѭ������
            while (!(Microphone.GetPosition(microphone) > 0)) { } // �ȴ���˷��ʼ��
            audioSource.Play(); // ����¼�Ƶ���Ƶ
        }
        else
        {
            Debug.LogWarning("û���ҵ���˷��豸");
        }
    }
}
