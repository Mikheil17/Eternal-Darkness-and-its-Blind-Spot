using UnityEngine;

public class RoomSwap : MonoBehaviour
{
    [Header("Room to disable")]
    public GameObject roomToHide;

    [Header("Room to enable")]
    public GameObject roomToShow;

    [Header("Tag of the player or collider that triggers the swap")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (roomToHide != null) roomToHide.SetActive(false);
        if (roomToShow != null) roomToShow.SetActive(true);

        // Prevent retriggering
        Destroy(gameObject);
    }
}