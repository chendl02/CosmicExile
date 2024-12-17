using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    public AudioSource audioSource; // 你的AudioSource组件
    private string microphone;

    void Start()
    {
        // 获取可用的麦克风设备
        if (Microphone.devices.Length > 0)
        {
            microphone = Microphone.devices[0]; // 使用第一个麦克风设备
            audioSource.clip = Microphone.Start(microphone, true, 10, 44100); // 开始录制
            audioSource.loop = true; // 设置为循环播放
            while (!(Microphone.GetPosition(microphone) > 0)) { } // 等待麦克风初始化
            audioSource.Play(); // 播放录制的音频
        }
        else
        {
            Debug.LogWarning("没有找到麦克风设备");
        }
    }
}
