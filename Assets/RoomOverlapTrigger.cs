using UnityEngine;

public class RoomOverlapTrigger : MonoBehaviour
{
    public Camera playerCamera;          // assign the XR Camera here
    public string enterRoomLayer = "RoomB_Layer";
    public string exitRoomLayer = "RoomA_Layer";
    private bool inRoomB = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRoomB = !inRoomB;
            SwitchRoomLayer();
        }
    }

    void SwitchRoomLayer()
    {
        if (playerCamera == null) return;

        int mask;
        if (inRoomB)
            mask = 1 << LayerMask.NameToLayer(enterRoomLayer);
        else
            mask = 1 << LayerMask.NameToLayer(exitRoomLayer);

        playerCamera.cullingMask = mask;
    }
}
