using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

public class MSCharacterController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator animator;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpSpeed = 15f;

    private Vector3 _moveVelocity = Vector3.zero;
    private readonly float _rotationSpeedFactor = 3f;

    private static readonly int DoubleJump = Animator.StringToHash("DoubleJump");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    void Update()
    {
        var hInput = Input.GetAxis("Horizontal");
        var vInput = Input.GetAxis("Vertical");

        _moveVelocity = new Vector3(hInput * speed, _moveVelocity.y, vInput * speed);

        if (Input.GetButtonDown("Jump"))
        {
            _moveVelocity.y = _characterController.isGrounded ? jumpSpeed : jumpSpeed * .75f;
            if (!_characterController.isGrounded) animator.SetTrigger(DoubleJump);
        }
        
        animator.SetBool(IsJumping, !_characterController.isGrounded);

        HandlePlayerRotation(hInput);

        _moveVelocity.y += gravity * Time.deltaTime;

        _characterController.Move(_moveVelocity * Time.deltaTime);
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
}