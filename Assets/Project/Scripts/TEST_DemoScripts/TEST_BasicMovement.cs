using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // Get input (WASD or Arrow Keys)
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

        // Build movement vector
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Move the player in world space
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}