using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class Scene1TransitionManager : MonoBehaviour
{
    public GameObject spaceShip; // Space Ship对象
    public float transitionRadius = 1f; // 切换场景的半径
    public float fadeDuration = 0.1f; // 黑屏持续时间

    private bool isTransitioning = false;
    private CanvasGroup fadeCanvasGroup;
    private List<GameObject> planetMeshes;

    void Start()
    {
        // 找到所有叫做PlanetMesh的物品
        transitionRadius = 1f;
        fadeDuration = 0.1f;
        planetMeshes = new List<GameObject>();
        //planetMeshes = GameObject.FindGameObjectsWithTag("Planet");
        planetMeshes.Add(GameObject.Find("Moon"));
        planetMeshes.Add(GameObject.Find("Venus"));
        //planetMeshes.Add(GameObject.Find("Earth"));
        planetMeshes.Add(GameObject.Find("Mars"));
        spaceShip = GameObject.Find("Space Ship");
        // 创建一个CanvasGroup用于黑屏效果
        GameObject fadeCanvas = new GameObject("FadeCanvas");
        Canvas canvas = fadeCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvasGroup = fadeCanvas.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 1f; // 初始透明度为0（完全透明）
    }

    void Update()
    {
        if (!isTransitioning)
        {
            foreach (GameObject planet in planetMeshes)
            {
                CelestialBody celestialBody = planet.GetComponent<CelestialBody>();
                Debug.Log("distance:"+Vector3.Distance(spaceShip.transform.position, celestialBody.transform.position));
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

        SceneManager.LoadScene("Scenes/"+name); // 替换为目标场景的名称

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
