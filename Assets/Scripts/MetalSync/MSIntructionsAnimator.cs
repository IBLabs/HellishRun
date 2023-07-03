using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MSIntructionsAnimator : MonoBehaviour
{
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float enterDuration;
    [SerializeField] private float moveOffset = 10f;

    [SerializeField] private Image targetImage;

    private RectTransform imageTransform;
    private Vector2 origPos;

    private bool shouldShow = false;
    private bool shouldHide = false;
    private bool killOnExit = false;

    // Start is called before the first frame update
    void Start()
    {
        imageTransform = GetComponent<RectTransform>();
        origPos = imageTransform.anchoredPosition;
        
        HideWithoutAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldShow)
        {
            shouldShow = false;
            StartCoroutine(startAnimationCoroutine(false, killOnExit));
        }

        if (shouldHide)
        {
            shouldHide = false;
            StartCoroutine(startAnimationCoroutine(true, killOnExit));
            killOnExit = false;
        }
    }

    public void HideWithoutAnimation()
    {
        imageTransform.anchoredPosition = origPos;
        targetImage.material.SetFloat("_FadeOffset", -3f);
        targetImage.material.SetFloat("_ScratchRatio", .07f);
    }

    public void SetVisible(bool visible, bool killOnExit)
    {
        shouldShow = visible;
        shouldHide = !visible;
        this.killOnExit = killOnExit;
    }

    private IEnumerator startAnimationCoroutine(bool exit, bool killOnExit)
    {
        float enterScratchRatio = -.15f;
        float exitScratchRatio = .01f;

        if (!exit) targetImage.material.SetFloat("_ScratchRatio", enterScratchRatio);
        float duration = enterDuration;
        float timer = 0;

        float fromValue = exit ? .07f : -3f;
        float toValue = exit ? -3f : .07f;

        Vector2 startPos = origPos - new Vector2(moveOffset, 0);
        while (timer <= duration)
        {
            float t = timer / duration;

            imageTransform.anchoredPosition = Vector2.Lerp(exit ? origPos : startPos, exit ? startPos : origPos,
                moveCurve.Evaluate(t));
            targetImage.material.SetFloat("_FadeOffset", Mathf.Lerp(fromValue, toValue, fadeCurve.Evaluate(t)));
            targetImage.material.SetFloat("_ScratchRatio",
                Mathf.Lerp(exit ? enterScratchRatio : exitScratchRatio, exit ? exitScratchRatio : enterScratchRatio,
                    fadeCurve.Evaluate(t)));

            timer += Time.deltaTime;

            yield return null;
        }

        if (exit) targetImage.material.SetFloat("_ScratchRatio", exitScratchRatio);
        if (killOnExit) 
            Destroy(gameObject);
    }
}
