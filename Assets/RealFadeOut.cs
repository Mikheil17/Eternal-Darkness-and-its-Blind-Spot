using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RealFadeOut : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;
    private bool isFading = false;

    void Awake()
    {
        if (fadeImage == null)
            fadeImage = GetComponent<Image>();
    }

    public void StartFadeOut()
    {
        if (!isFading)
            StartCoroutine(FadeToWhite());
    }

    private IEnumerator FadeToWhite()
    {
        isFading = true;
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
        isFading = false;
    }
}