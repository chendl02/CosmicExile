using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StarDome : MonoBehaviour {

    public MeshRenderer starPrefab;
    public Vector2 radiusMinMax;
    public int count = 1000;
    const float calibrationDst = 2000;
    public Vector2 brightnessMinMax;

    public Image backgroundImage;   
    public float fadeDuration = 1f; 

    Camera cam;

    void Start () {
        // scene loading
        StartCoroutine(FadeFromBlack());

        cam = Camera.main;
        //var sw = System.Diagnostics.Stopwatch.StartNew ();
        float starDst = cam.farClipPlane - radiusMinMax.y;
        float scale = starDst / calibrationDst;

        for (int i = 0; i < count; i++) {
            MeshRenderer star = Instantiate (starPrefab, Random.onUnitSphere * starDst, Quaternion.identity, transform);
            float t = SmallestRandomValue (6);
            star.transform.localScale = Vector3.one * Mathf.Lerp (radiusMinMax.x, radiusMinMax.y, t) * scale;
            star.material.color = Color.Lerp (Color.black, star.material.color, Mathf.Lerp (brightnessMinMax.x, brightnessMinMax.y, t));
        }
        //Debug.Log (sw.ElapsedMilliseconds);
    }

    float SmallestRandomValue (int iterations) {
        float r = 1;
        for (int i = 0; i < iterations; i++) {
            r = Mathf.Min (r, Random.value);
        }
        return r;
    }

    void LateUpdate () {
        if (cam != null) {
            transform.position = cam.transform.position;
        }
    }
    private IEnumerator FadeFromBlack()
    {
        Color color = backgroundImage.color; 
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            color.a = Mathf.Clamp01(1f - (t / fadeDuration));
            backgroundImage.color = color; 
            // canvasGroup.color.a = Mathf.Clamp01(1f - (t / fadeDuration));
            yield return null;
        }
        color.a = 0f;
        backgroundImage.color = color; 
        backgroundImage.gameObject.SetActive(false);
    }
}