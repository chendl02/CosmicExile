using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    public Text text;
    public float speed = 2f;
    public float minAlpha = 0.5f;  
    public float maxAlpha = 1f;    

    void Update()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * speed, 1f));
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}