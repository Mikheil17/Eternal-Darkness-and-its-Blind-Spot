using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [Header("Assign the player's head (XR camera)")]
    public Transform playerHead;

    [Header("UI fade image (full-screen white image)")]
    public Image fadeImage;

    [Header("Fade settings")]
    public float fadeDuration = 2f;

    private bool triggered = false;

    private void Start()
    {
        // Make sure fade image starts invisible but active
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[FadeToWhite] OnTriggerEnter called with {other.name}");
        if (triggered || !other.CompareTag("Player"))
            return;

        triggered = true;
        StartCoroutine(FadeToWhite());
    }

    private IEnumerator FadeToWhite()
    {
        if (fadeImage == null)
            yield break;

        Color color = fadeImage.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // ensure full white at end
        color.a = 1f;
        fadeImage.color = color;
    }
}