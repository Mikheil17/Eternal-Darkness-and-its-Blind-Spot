using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public RealFadeOut fadeOutScript;  // Reference to your VRFadeOut script
    public float fadeDuration = 8f;  // Fade length in seconds

    private void OnTriggerEnter(Collider other)
    {
        // Detect the player's rig or camera
        if (other.CompareTag("MainCamera") || other.CompareTag("Player"))
        {
            fadeOutScript.fadeDuration = fadeDuration;
            fadeOutScript.StartFadeOut();
        }
    }
}