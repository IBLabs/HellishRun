using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float rotateDuration = 0.2f;
    public float moveSpeed = 5f;
    public float rotationAngle = 90f; // The angle of rotation when the player moves
    public float rotationSpeed = 10f;
    
    
    private Vector3 targetPosition;
    private bool isMoving = false;

    private float targetRot = 0f;

    void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.leftArrowKey.isPressed)
        {
            transform.position += Vector3.forward * (moveSpeed * Time.deltaTime);
            targetRot = 90 - 45f;
        } 
        else if (keyboard.rightArrowKey.isPressed)
        {
            transform.position += Vector3.back * (moveSpeed * Time.deltaTime);
            targetRot = 90 + 45f;
        }
        else
        {
            targetRot = 90;
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(targetRot, Vector3.up), rotationSpeed * Time.deltaTime);

        return;
        
        if (!isMoving)
        {
            if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                StartCoroutine(MovePlayer(new Vector3(transform.position.x - moveSpeed, transform.position.y, transform.position.z), -rotationAngle));
            }
            else if (keyboard.rightArrowKey.wasPressedThisFrame)
            {
                StartCoroutine(MovePlayer(new Vector3(transform.position.x + moveSpeed, transform.position.y, transform.position.z), rotationAngle));
            }
        }
    }

    IEnumerator MovePlayer(Vector3 targetPosition, float rotationAngle)
    {
        isMoving = true;
        // Calculate the rotation needed and divide by the duration to get the speed
        float rotationSpeed = rotationAngle / rotateDuration;

        // Rotate player
        for(float t = 0; t < rotateDuration; t += Time.deltaTime)
        {
            transform.Rotate(Vector3.up, rotationAngle * Time.deltaTime);
            yield return null;
        }

        // Move player horizontally
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Rotate player back
        for(float t = 0; t < rotateDuration; t += Time.deltaTime)
        {
            transform.Rotate(Vector3.up, -rotationAngle * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}