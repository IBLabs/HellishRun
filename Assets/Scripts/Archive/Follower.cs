using System.Collections;
using System.Collections.Generic;
using PathCreation;
using Unity.Mathematics;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float DistanceTravelled => _distanceTravelled;
    public Quaternion LocalRotation => rotator.localRotation;

    public PathCreator Creator;
    public float speed = 5;
    
    public float turnDuration = .3f;
    public float turnAmount = 30;
    public Transform rotator;
    public AnimationCurve rotationCurve;

    public float jumpDuration = 1;
    public float jumpHeight = 1;
    public Transform jumper;
    public AnimationCurve jumpCurve;

    private float turnTimer = Mathf.Infinity;
    private Quaternion currentRot;
    private Quaternion targetRot;
    
    private float _distanceTravelled;

    private float jumpTimer = Mathf.Infinity;
    private float currentY;
    private float targetY;

    void Update()
    {
        _distanceTravelled += speed * Time.deltaTime;
        transform.position = Creator.path.GetPointAtDistance(_distanceTravelled);
        transform.rotation = Creator.path.GetRotationAtDistance(_distanceTravelled);
        
        var rotation = rotator.localRotation;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (turnTimer < turnDuration) return;
            
            turnTimer = 0;
            currentRot = rotator.localRotation;
            targetRot = Quaternion.AngleAxis(360 / -turnAmount, Vector3.forward) * rotation;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (turnTimer < turnDuration) return;
            
            turnTimer = 0;
            currentRot = rotator.localRotation;
            targetRot = Quaternion.AngleAxis(360 / turnAmount, Vector3.forward) * rotation;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpTimer < jumpDuration) return;
            
            jumpTimer = 0;
            currentY = jumper.localPosition.y;
            targetY = currentY + jumpHeight;
        }

        if (turnTimer <= turnDuration)
        {
            float percent = turnTimer / turnDuration;
            float targetPercent = rotationCurve.Evaluate(percent);
            rotator.localRotation = Quaternion.Slerp(currentRot, targetRot, targetPercent);
            turnTimer += Time.deltaTime;

            if (turnTimer >= turnDuration)
                rotator.localRotation = targetRot;
        }

        if (jumpTimer <= jumpDuration)
        {
            float percent = jumpTimer / jumpDuration;
            float targetPercent = jumpCurve.Evaluate(percent);

            var pos = jumper.localPosition;
            pos.y = Mathf.Lerp(currentY, targetY, targetPercent);
            jumper.localPosition = pos;

            jumpTimer += Time.deltaTime;

            if (jumpTimer >= jumpDuration)
            {
                pos.y = currentY;
                jumper.localPosition = pos;
            }
        }
    }
}
