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

    [Header("Flashlight Fix")]
    [Tooltip("Reference to the player's flashlight GameObject (the one with a Light component)")]
    public GameObject flashlightObject;

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

        // Ensure flashlight light is active if it got turned off mid-flicker
        if (flashlightObject != null)
        {
            Light lightComp = flashlightObject.GetComponent<Light>();
            if (lightComp != null && !lightComp.enabled)
            {
                Debug.Log("[RoomSwap] Flashlight was off during swap — re-enabling spotlight.");
                lightComp.enabled = true;
            }

            // Also ensure the GameObject itself is active (in case something disabled it)
            if (!flashlightObject.activeSelf)
            {
                flashlightObject.SetActive(true);
                Debug.Log("[RoomSwap] Flashlight GameObject was inactive — reactivated.");
            }
        }

        // Prevent retriggering
        Destroy(gameObject);
    }
}
