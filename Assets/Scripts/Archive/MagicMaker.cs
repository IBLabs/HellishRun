using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MagicMaker : MonoBehaviour
{
    public Transform transform1; // First transform object to animate
    public Transform transform2; // Second transform object to animate
    public float xDistance = 10f; // The X distance for the first transform object
    public Vector2 yzRange = new Vector2(-5f, 5f); // The YZ range for the second transform object
    public float speed1 = 2f; // The speed for animating the first transform object
    public float speed2 = 2f; // The speed for animating the second transform object

    private bool isAnimating = false; // Flag to track whether an animation is currently in progress

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerAnimation();
        }
    }

    // Call this function to trigger the animation
    public void TriggerAnimation()
    {
        if (!isAnimating)
        {
            StartCoroutine(AnimateTransforms());
        }
    }

    private IEnumerator AnimateTransforms()
    {
        isAnimating = true;

        // Position the first transform object
        transform1.position = new Vector3(xDistance, 0, 0);

        // Position the second transform object
        float y = Random.Range(yzRange.x, yzRange.y);
        float z = Random.Range(yzRange.x, yzRange.y);
        transform2.position = new Vector3(0, y, z);

        // Animate the first transform object back to the world center
        while (transform1.position != Vector3.zero)
        {
            transform1.position = Vector3.MoveTowards(transform1.position, Vector3.zero, speed1 * Time.deltaTime);
            yield return null;
        }

        // Animate the second transform object back to the world center
        while (transform2.position != Vector3.zero)
        {
            transform2.position = Vector3.MoveTowards(transform2.position, Vector3.zero, speed2 * Time.deltaTime);
            yield return null;
        }

        isAnimating = false;
    }
}