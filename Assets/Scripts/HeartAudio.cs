using UnityEngine;

public class PlayLoopOnTrigger : MonoBehaviour
{
    [Header("Tag of the player or collider that triggers the sound")]
    public string playerTag = "Player";

    [Header("Audio Source (assign in Inspector)")]
    public AudioSource audioSource; // 🎵 Drag your AudioSource here

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
