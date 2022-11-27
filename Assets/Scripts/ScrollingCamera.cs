using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCamera : MonoBehaviour
{
    public float scrollSpeed;

    private void FixedUpdate()
    {
        // Move the camera scroll speed on x axis
        transform.position = new Vector3(
            transform.position.x + scrollSpeed * Time.deltaTime,
            transform.position.y,
            transform.position.z
        );
    }
}
