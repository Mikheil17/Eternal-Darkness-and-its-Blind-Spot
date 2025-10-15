using UnityEngine;

public class RoomSwap : MonoBehaviour
{
    [Header("Room to disable")]
    public GameObject roomToHide;

    [Header("Room to enable")]
    public GameObject roomToShow;

    [Header("Tag of the player or collider that triggers the swap")]
    public string playerTag = "Player";

    [Header("Audio")]
    public AudioSource ambientAudio;
    public bool playOnSwap = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Swap rooms
        if (roomToHide != null) roomToHide.SetActive(false);
        if (roomToShow != null) roomToShow.SetActive(true);

        // Play looping ambiance
        if (playOnSwap && ambientAudio != null)
        {
            ambientAudio.loop = true;
            ambientAudio.Play();
        }

        // Prevent retriggering
        Destroy(gameObject);
    }
}
