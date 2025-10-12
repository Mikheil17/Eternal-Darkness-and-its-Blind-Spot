using UnityEngine;

public class DebugMovement : MonoBehaviour
{
    public float speed = 3f;
    public float lookSpeed = 2f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.Self);

        if (Input.GetMouseButton(1))
            transform.Rotate(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f);
    }
}
