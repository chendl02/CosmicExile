using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorChanger : MonoBehaviour
{
    public Text text;
    public Color color1 = Color.blue;
    public Color color2 = Color.cyan;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        text.color = Color.Lerp(color1, color2, t);
    }
}