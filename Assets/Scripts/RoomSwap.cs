using UnityEngine;

public class RoomSwap : MonoBehaviour
{
    [Header("Assign your rooms here (in order)")]
    public GameObject room1;
    public GameObject room2;
    public GameObject room3;

    [Header("Tag of the player or collider that triggers the swap")]
    public string playerTag = "Player";

    private int currentRoom = 1;
    private bool triggered = false;

    private void Start()
    {
        // Make sure only room1 starts active
        if (room1 != null) room1.SetActive(true);
        if (room2 != null) room2.SetActive(false);
        if (room3 != null) room3.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag(playerTag))
            return;

        triggered = true; // prevents multiple activations

        // Move to next room
        currentRoom++;

        if (currentRoom == 2)
        {
            if (room1 != null) room1.SetActive(false);
            if (room2 != null) room2.SetActive(true);
        }
        else if (currentRoom == 3)
        {
            if (room2 != null) room2.SetActive(false);
            if (room3 != null) room3.SetActive(true);
        }
        
    }
}

