using UnityEngine;

public class RoomSwap : MonoBehaviour
{
    [Header("Assign your rooms here")]
    public GameObject room1;
    public GameObject room2;

    [Header("Tag of the player or collider that triggers the swap")]
    public string playerTag = "Player";

    private bool hasSwapped = false;

    private void OnTriggerEnter(Collider other)
    {
        // only swap once and only if the player enters
        if (!hasSwapped && other.CompareTag(playerTag))
        {
            hasSwapped = true;
            if (room1 != null) room1.SetActive(false);
            if (room2 != null) room2.SetActive(true);
        }
    }
}
