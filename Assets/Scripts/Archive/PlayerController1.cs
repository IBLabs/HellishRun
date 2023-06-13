using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float spinSpeed = 5f;
    public float maxSpinSpeed = 20f;
    public float timeToMaxSpeed = 2f;
    public float launchForce = 500f; // Force with which the player will be launched

    private float currentSpinSpeed = 0f;
    private float currentTime = 0f;

    private Rigidbody2D rb; // Rigidbody component of the player

    // Flag to track if the player is spinning
    private bool isSpinning = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody component
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            currentTime += Time.deltaTime;
            currentSpinSpeed = Mathf.Lerp(0, maxSpinSpeed, currentTime / timeToMaxSpeed);
            currentSpinSpeed = Mathf.Clamp(currentSpinSpeed, 0, maxSpinSpeed);
            rb.MoveRotation(rb.rotation + currentSpinSpeed * Time.deltaTime);

            isSpinning = true; // Set the flag to true
        }
        else
        {
            if (isSpinning) // Check if the player was spinning
            {
                Launch(); // Launch the player
            }

            // Reset the current time and current spin speed
            currentTime = 0;
            currentSpinSpeed = 0;

            isSpinning = false; // Reset the flag
        }
    }

    void Update()
    {
        
    }

    void Launch()
    {
        // Calculate the direction to launch the player in
        Vector2 direction = transform.up;

        // Apply a force to the Rigidbody in the calculated direction
        rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
    }
}