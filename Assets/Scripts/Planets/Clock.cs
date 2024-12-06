using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    // Start is called before the first frame update
    public Text time_text;
    public float time;

    void Awake()
    {
        Time.fixedDeltaTime = Universe.physicsTimeStep;
    }

    void FixedUpdate()
    {
        time += Universe.physicsTimeStep * Universe.timeCoefficient / 3600.0f / 24.0f;
        time_text.text = time.ToString("F0") + " Days";
    }
}
