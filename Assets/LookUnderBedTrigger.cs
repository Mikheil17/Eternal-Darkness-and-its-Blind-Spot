using UnityEngine;
using System.Collections;

public class LookUnderBedTrigger : MonoBehaviour
{
    [Header("Assign the player's head (XR camera)")]
    public Transform playerHead;

    [Header("Object to remove or hide")]
    public GameObject targetObject;

    [Header("Audio that plays after delay")]
    public AudioSource audioSource;     // assign an AudioSource in the Inspector
    public float delayBeforeAudio = 3f; // seconds before sound plays

    [Header("Destroy or just hide?")]
    public bool destroyInstead = false;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (targetObject != null)
            {
                if (destroyInstead)
                    Destroy(targetObject);
                else
                    targetObject.SetActive(false);
            }

            if (audioSource != null)
                StartCoroutine(PlayDelayedAudio());
        }
    }

    private IEnumerator PlayDelayedAudio()
    {
        yield return new WaitForSeconds(delayBeforeAudio);
        audioSource.Play();
    }
}
