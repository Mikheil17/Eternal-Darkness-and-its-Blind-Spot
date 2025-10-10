using UnityEngine;

public class RoomSwapTrigger : MonoBehaviour
{
    [Header("Room Settings")]
    public GameObject currentRoom;  // The room currently active
    public GameObject nextRoom;     // The room to activate next

    [Header("Optional Player Tag")]
    public string playerTag = "Player"; // The tag on your XR Rig or main camera parent

    [Header("Transition Settings")]
    public float fadeDuration = 0.5f; // Optional: for future fade effect

    private bool hasSwapped = false; // Prevents double-triggering

    private void OnTriggerEnter(Collider other)
    {
        if (hasSwapped) return;

        // Check if the collider belongs to the player
        if (other.CompareTag(playerTag))
        {
            SwapRooms();
            hasSwapped = true;
        }
    }

    private void SwapRooms()
    {
        if (currentRoom != null)
            currentRoom.SetActive(false);

        if (nextRoom != null)
            nextRoom.SetActive(true);

        Debug.Log($"Room swapped: {currentRoom?.name} → {nextRoom?.name}");
    }
}
