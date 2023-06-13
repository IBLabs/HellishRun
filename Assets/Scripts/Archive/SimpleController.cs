using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    // Movement parameters
    public float maxSpeed = 10.0f; // The maximum speed the character can reach
    public float acceleration = 2.0f; // The rate at which the character speeds up
    public float rotationSpeed = 5.0f; // The speed at which the character rotates

    private Rigidbody rb; // The Rigidbody component
    private Vector3 inputVector; // The vector to store user inputs
    private Vector3 velocity; // The current velocity of the character
    private Animator animator; // The Animator component

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Get user inputs
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector based on user inputs
        inputVector = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Increase the character's velocity based on the input vector, but don't exceed maxSpeed
        velocity = Vector3.Lerp(velocity, inputVector * maxSpeed, acceleration * Time.deltaTime);

        // Rotate the character to face the direction of movement
        if (inputVector != Vector3.zero) 
        {
            Quaternion toRotation = Quaternion.LookRotation(inputVector, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Control the animations
        animator.SetBool("IsRunning", inputVector != Vector3.zero);
    }

    private void FixedUpdate()
    {
        // Move the character
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }
}
