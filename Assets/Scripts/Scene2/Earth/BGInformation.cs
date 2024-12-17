using UnityEngine;
using TMPro;

public class IntroMessage : MonoBehaviour
{
    public TextMeshProUGUI introText; // 引用 TextMeshPro 文本
    public float displayDuration = 8f; // 文本显示时长
    public float fadeDuration = 2f;    // 淡出时长

    private float timer;

    void Start()
    {
        // 设置新的提示文字
        if (introText != null)
        {
            introText.text = "After thousands of years of abandonment of the Earth, the environment has changed drastically.\n\nYou need to explore this planet and collect enough fuel to restart the airship.\n\nMeanwhile, you are located in a dangerous environment, so you need to escape from the viruses.";
            introText.alpha = 1f;
            introText.alignment = TextAlignmentOptions.Center; // 文本居中
        }
        timer = displayDuration;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StartCoroutine(FadeOutText());
            }
        }
    }

    private System.Collections.IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            introText.alpha = alpha;
            yield return null;
        }

        // 完全淡出后隐藏文本
        introText.gameObject.SetActive(false);
    }
}
