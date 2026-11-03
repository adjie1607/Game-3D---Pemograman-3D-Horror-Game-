using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    private void Start()
    {
        if (fadeCanvas != null)
            StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
    }
}
