using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public float fadeDuration = 1f; 
    public string sceneToLoad;
    public CanvasGroup canvasGroup;   
    public Text loadingText; 
    public Button startButton;       

    private void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
        loadingText.gameObject.SetActive(false);

        startButton.gameObject.SetActive(true);
        startButton.interactable = true;
        startButton.onClick.AddListener(() => SwitchScene(sceneToLoad)); 
    }

    public void SwitchScene(string sceneName)
    {
        StartCoroutine(HandleSceneTransition(sceneName));
    }

    private IEnumerator HandleSceneTransition(string sceneName)
    {
        startButton.interactable = false;

        yield return StartCoroutine(FadeToBlack());

        loadingText.gameObject.SetActive(true);

        yield return StartCoroutine(LoadSceneAsync(sceneName));

        yield return StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeToBlack()
    {
        canvasGroup.gameObject.SetActive(true);
        // loadingText.gameObject.SetActive(false); 
        loadingText.gameObject.SetActive(true);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        loadingText.text = "Loading...";

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                // loadingText.text = "Loading...";
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator FadeFromBlack()
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Clamp01(1f - (t / fadeDuration));
            yield return null;
        }

        canvasGroup.alpha = 0f; 
        canvasGroup.gameObject.SetActive(false);
    }

}
