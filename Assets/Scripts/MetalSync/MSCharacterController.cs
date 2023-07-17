using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MSCharacterController : MonoBehaviour
{
    public static event Action PlayerTookHit;
    
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionAsset input;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<AudioClip> jumpClips;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float smoothInputSpeed = .2f;

    private Vector3 _moveVelocity = Vector3.zero;
    private readonly float _rotationSpeedFactor = 3f;

    private static readonly int DoubleJump = Animator.StringToHash("DoubleJump");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    private int currentJumpClip;

    void Update()
    {
        Vector2 movement = input["Move"].ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, movement, ref smoothInputVelocity, smoothInputSpeed);

        float hInput = currentInputVector.x;
        float vInput = currentInputVector.y;

        _moveVelocity = new Vector3(hInput * speed, _moveVelocity.y, vInput * speed);

        animator.SetBool(IsJumping, !_characterController.isGrounded);

        HandlePlayerRotation(hInput);

        _moveVelocity.y += gravity * Time.deltaTime;

        _characterController.Move(_moveVelocity * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        int newJumpClipIndex;
        do newJumpClipIndex = Random.Range(0, jumpClips.Count);
        while (newJumpClipIndex == currentJumpClip);
        currentJumpClip = newJumpClipIndex;
            
        AudioClip targetJumpClip = jumpClips[currentJumpClip];
        audioSource.PlayOneShot(targetJumpClip);
        
        _moveVelocity.y = _characterController.isGrounded ? jumpSpeed : jumpSpeed * .75f;
        if (!_characterController.isGrounded) animator.SetTrigger(DoubleJump);
    }

    public void Move(InputAction.CallbackContext context)
    {
        
    }

    private void HandlePlayerRotation(float hInput)
    {
        // rotate the player based on the direction of movement
        if (hInput != 0)
        {
            Quaternion rotation = Quaternion.Euler(0, rotationSpeed * hInput, 0);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed * _rotationSpeedFactor);
        }

        // reset the player rotation when not moving
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity,
                Time.deltaTime * speed * _rotationSpeedFactor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BoxCollider[] colliders = {
            other.gameObject.GetComponentInParent<BoxCollider>(),
            other.gameObject.GetComponentInChildren<BoxCollider>()
        };
        
        foreach (var collider in colliders)
            if (collider != null)
            {
                collider.enabled = false;
            }
                
        
        if (other.CompareTag("Obstacle"))
        {
            PlayerTookHit?.Invoke();
        }
    }
}