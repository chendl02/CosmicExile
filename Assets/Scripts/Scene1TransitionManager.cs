using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class Scene1TransitionManager : MonoBehaviour
{
    public GameObject spaceShip; // Space Ship����
    public float transitionRadius = 0.1f; // �л������İ뾶
    public float fadeDuration = 0.1f; // ��������ʱ��

    private bool isTransitioning = false;
    private CanvasGroup fadeCanvasGroup;
    private List<GameObject> planetMeshes;

    void Start()
    {
        // �ҵ����н���PlanetMesh����Ʒ
        fadeDuration = 0.1f;
        planetMeshes = new List<GameObject>();
        //planetMeshes = GameObject.FindGameObjectsWithTag("Planet");
        planetMeshes.Add(GameObject.Find("Moon"));
        planetMeshes.Add(GameObject.Find("Venus"));
        //planetMeshes.Add(GameObject.Find("Earth"));
        planetMeshes.Add(GameObject.Find("Mars"));
        spaceShip = GameObject.Find("Space Ship");
        // ����һ��CanvasGroup���ں���Ч��
        GameObject fadeCanvas = new GameObject("FadeCanvas");
        Canvas canvas = fadeCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvasGroup = fadeCanvas.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 1f; // ��ʼ͸����Ϊ0����ȫ͸����
    }

    void Update()
    {
        if (!isTransitioning)
        {
            foreach (GameObject planet in planetMeshes)
            {
                CelestialBody celestialBody = planet.GetComponent<CelestialBody>();
                //Debug.Log("distance:"+Vector3.Distance(spaceShip.transform.position, celestialBody.transform.position));
                if (Vector3.Distance(spaceShip.transform.position, celestialBody.transform.position) <= celestialBody.radius+transitionRadius)
                {
                    Debug.Log("distance:" + Vector3.Distance(spaceShip.transform.position, celestialBody.transform.position));
                    if (celestialBody.name == "Moon") { StartCoroutine(FadeAndSwitchScene("Lunar")); }
                    else { StartCoroutine(FadeAndSwitchScene(celestialBody.name)); }
                    break;
                }
            }
        }
    }

    IEnumerator FadeAndSwitchScene(string name)
    {
        isTransitioning = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene("Scenes/"+name); // �滻ΪĿ�곡��������

        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        isTransitioning = false;
    }
}
