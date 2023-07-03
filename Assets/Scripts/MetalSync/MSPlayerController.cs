using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MSPlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // The speed at which the player moves
    public float rotationMoveSpeedFactor = 1.2f;
    public float jumpForce = 5.0f; // The force applied when the player jumps
    public float padding = 1.0f; // The padding from the edge of the track
    public float groundCheckRadius = 0.2f; // The radius of the ground check sphere
    public LayerMask groundLayer; // The layer considered as ground
    public float turnAngle = 20.0f; // The angle to turn the player when moving left or right
    public float verticalBounds = 1f;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip doubleJumpClip;

    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private AnimationCurve fallCurve;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float jumpHeight = 2f;
    
    private float targetJumpY = 0f;
    private float targetMoveX = 0f;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    
    private Animator animator;
    
    private Rigidbody rb; // The Rigidbody component
    private bool isGrounded; // Whether the player is on the ground
    private float trackWidth; // The width of the track

    private float originalZPosition;

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Calculate the width of the track
        trackWidth = (GameObject.FindObjectOfType<MSObstacleTrackController>().numLanes * FindObjectOfType<MSObstacleTrackController>().laneWidth) / 2 - padding;

        originalZPosition = transform.position.z;
    }

    private void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);
        
        // Update the player's jump animation
        animator.SetBool("IsJumping", !isGrounded);

        // Get user inputs
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        // Rotate the player based on the direction of movement
        if (moveHorizontal != 0)
        {
            Quaternion rotation = Quaternion.Euler(0, turnAngle * moveHorizontal, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * moveSpeed * rotationMoveSpeedFactor);
        }
        else
        {
            // Reset the player rotation when not moving
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * moveSpeed * rotationMoveSpeedFactor);
        }

        // Move the player horizontally
        float newXPosition = transform.position.x + moveHorizontal * moveSpeed * Time.deltaTime;
        targetMoveX = Mathf.Clamp(newXPosition, -trackWidth, trackWidth);
        
        // Move the player vertically
        float newZPosition = transform.position.z + moveVertical * moveSpeed * Time.deltaTime;
        newZPosition = Mathf.Clamp(newZPosition, originalZPosition - verticalBounds * 1.5f,
            originalZPosition + verticalBounds);

        // transform.position = new Vector3(newXPosition, transform.position.y, newZPosition);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        // if (rb.velocity.y < 0)
        // {
        //     rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        // }
        // else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
        // {
        //     rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        // }
    }

    private void FixedUpdate()
    {
        if (isJumping || isDoubleJumping)
        {
            rb.MovePosition(new Vector3(targetMoveX, targetJumpY, rb.transform.position.z));
        }
        else
        {
            rb.MovePosition(new Vector3(targetMoveX, rb.transform.position.y, rb.transform.position.z));
        }
    }

    public void Jump()
    {
        /*
        // Make the player jump if they're on the ground
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _audioSource.PlayOneShot(jumpClip);
        }
        else
        {
            animator.SetTrigger("DoubleJump");
            rb.AddForce(Vector3.up * (jumpForce * .75f), ForceMode.Impulse);
            _audioSource.PlayOneShot(doubleJumpClip);
        }
        */

        if (isGrounded)
        {
            StartCoroutine(JumpCoroutine());
        }
        else
        {
            StopCoroutine("JumpCoroutine");
            StartCoroutine(DoubleJumpCoroutine());
        }
    }

    private IEnumerator JumpCoroutine()
    {
        float duration = jumpDuration / 2;
        float timer = 0f;

        float sourceY = rb.position.y;
        float targetY = sourceY + jumpHeight;

        isJumping = true;
        
        while (timer <= duration && !isDoubleJumping)
        {
            float t = timer / duration;
            float curveT = jumpCurve.Evaluate(t);

            targetJumpY = Mathf.LerpUnclamped(sourceY, targetY, curveT);
            
            Debug.Log($"[TEST] target Y is: {targetJumpY}, t is: {t}, curve t is: {curveT}");

            timer += Time.deltaTime;
            
            yield return null;
        }

        isJumping = false;
    }

    private IEnumerator DoubleJumpCoroutine()
    {
        isJumping = false;
        
        float duration = jumpDuration;
        float timer = 0f;

        float sourceY = rb.position.y;
        float targetY = sourceY + jumpHeight * 2;

        isDoubleJumping = true;
        
        while (timer <= duration)
        {
            float t = timer / duration;
            float curveT = jumpCurve.Evaluate(t);

            targetJumpY = Mathf.LerpUnclamped(sourceY, targetY, curveT);
            
            Debug.Log($"[DOUBLE JUMP] target Y is: {targetJumpY}, t is: {t}, curve t is: {curveT}");

            timer += Time.deltaTime;
            
            yield return null;
        }

        isDoubleJumping = false;
    }

    public void Hit()
    {
        // Perform the hit action
        // Implementation depends on your specific requirements
    }
}