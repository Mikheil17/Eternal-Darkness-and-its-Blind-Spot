using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [Header("References")]
    public Image fadeImage;                 // The UI Image that covers the screen (white, alpha = 0)
    public AudioSource audioSource;         // The audio source to play when triggered
    public float fadeDuration = 8f;         // Fade duration in seconds
    public bool quitOnEnd = true;           // Whether to quit the game after the sound ends

    private bool hasTriggered = false;
    private float fadeTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        // Trigger only once, when player hits the collider
        if (hasTriggered) return;

        // Check if the object entering is the XR Rig or player head
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            audioSource.Play();
            StartCoroutine(FadeOutAndQuit());
        }
    }

    private System.Collections.IEnumerator FadeOutAndQuit()
    {
        Color c = fadeImage.color;

        // Fade to white
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // Wait for the audio to finish
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Quit the game (works in build, not in Editor)
        if (quitOnEnd)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}