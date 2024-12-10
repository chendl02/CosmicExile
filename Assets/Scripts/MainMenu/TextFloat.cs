using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    public Text text;
    public float speed = 2f;

    void Update()
    {
        // 动态调整透明度
        float alpha = Mathf.PingPong(Time.time * speed, 1f);
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}