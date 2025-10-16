using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    [Header("References")]
    public Image fadeImage;                       // The UI Image that covers the screen (white, alpha = 0)

    [Tooltip("Primary audio to play during fade")]
    public AudioSource audioSource;               // The main audio to play when triggered

    [Tooltip("Other audios to stop when triggered (like ambient sounds, music, etc.)")]
    public AudioSource[] otherAudioSources;       // Any other audios to stop immediately

    public float fadeDuration = 8f;               // Fade duration in seconds
    public bool quitOnEnd = true;                 // Whether to quit the game after the sound ends

    private bool hasTriggered = false;
    private float fadeTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        // Trigger only once
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            // Stop all secondary audios immediately
            foreach (AudioSource a in otherAudioSources)
            {
                if (a != null && a.isPlaying)
                    a.Stop();
            }

            // Play the main fade audio
            if (audioSource != null)
                audioSource.Play();

            StartCoroutine(FadeOutAndQuit());
        }
    }

    private IEnumerator FadeOutAndQuit()
    {
        if (fadeImage == null)
            yield break;

        Color c = fadeImage.color;
        fadeTimer = 0f;

        // Fade to white
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // Wait for main audio to finish (if assigned)
        if (audioSource != null)
        {
            while (audioSource.isPlaying)
                yield return null;
        }

        // Quit game (works only in build)
        if (quitOnEnd)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
