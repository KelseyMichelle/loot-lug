using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBack : MonoBehaviour
{
    public Rigidbody2D rb;

    public float velocityApplied;

    Camera mainCam = Camera.main;
    private float mainCamHeight;
    private float mainCamWidth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Calculate on-screen width and height in pixels
        mainCamHeight = 2f * mainCam.orthographicSize;
        mainCamWidth = mainCamHeight * mainCam.aspect;
    }

    private void FixedUpdate()
    {
        float cameraDistX = mainCam.transform.position.x - transform.position.x;
        float cameraDistY = mainCam.transform.position.y - transform.position.y;

        float velocityAdjustment = velocityApplied * Time.deltaTime;

        if (cameraDistX > mainCamWidth / 2)
        {
            rb.velocity = new Vector2(rb.velocity.x - velocityAdjustment, rb.velocity.y);
        } else if (cameraDistX < -mainCamWidth / 2)
        {
            rb.velocity = new Vector2(rb.velocity.x + velocityAdjustment, rb.velocity.y);
        } else if (cameraDistY > mainCamHeight / 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - velocityAdjustment);
        } else if (cameraDistY < -mainCamHeight / 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + velocityAdjustment);
        }
    }
}
