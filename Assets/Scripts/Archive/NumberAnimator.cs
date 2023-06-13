using UnityEngine;
using System.Collections;

public class NumberAnimator : MonoBehaviour
{
    public AnimationCurve curve;
    
    public Vector2 durationRange = new Vector2(1, 5);  // Range for the random duration of the animation
    public Vector2 targetRange = new Vector2(0, 100);  // Range for the target number values
    public Vector2 number2TargetRange = new Vector2(0, 100);
    public Vector2 delayRange = new Vector2(1, 5);     // Range for the random delay between animations

    public static float Number1, Number2; // Numbers to animate

    private void Start()
    {
        // Start the coroutine
        StartCoroutine(AnimateNumbers());
    }

    private IEnumerator AnimateNumbers()
    {
        while (true)
        {
            // Get random target numbers
            float targetNumber1 = Random.Range(targetRange.x, targetRange.y);
            float targetNumber2 = Random.Range(number2TargetRange.x, number2TargetRange.y);

            // Get random duration
            float duration = Random.Range(durationRange.x, durationRange.y);

            // Animate numbers towards the target
            yield return StartCoroutine(Animate(duration, targetNumber1, targetNumber2));

            // Animate numbers back to 0
            yield return StartCoroutine(Animate(duration, 0, 0));

            // Wait for a random delay before the next animation
            float delay = Random.Range(delayRange.x, delayRange.y);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator Animate(float duration, float targetNumber1, float targetNumber2)
    {
        // Store the start time
        float startTime = Time.time;

        // Store the starting numbers
        float startNumber1 = Number1;
        float startNumber2 = Number2;

        while (Time.time < startTime + duration)
        {
            // Calculate how far along we are in the duration
            float t = (Time.time - startTime) / duration;

            float targetT = curve.Evaluate(t);

            // Lerp the numbers
            Number1 = Mathf.Lerp(startNumber1, targetNumber1, targetT);
            Number2 = Mathf.Lerp(startNumber2, targetNumber2, targetT);

            yield return null;
        }

        // Ensure the numbers reach the target values at the end of the animation
        Number1 = targetNumber1;
        Number2 = targetNumber2;

        // Do something with the animated numbers
        Debug.Log("Number1: " + Number1 + ", Number2: " + Number2);
    }
}
