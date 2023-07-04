using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChainsRollupController : MonoBehaviour
{
    [SerializeField] private Image chainImage;

    [SerializeField] private float duration = 5f;
    [SerializeField] private float offset = 1f;
    [SerializeField] private Ease easing = Ease.InOutCubic;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private RectTransform uiRectTransform;
    
    void Start()
    {
        
    }

    public void PlayScrollAnimationSoundAnimationEvent()
    {
        _audioSource.Play();
    }

    public void PlayScrollAnimationEvent()
    {
        StartCoroutine(ScrollAnimationCoroutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayScrollAnimationEvent();
        }
    }

    private IEnumerator ScrollAnimationCoroutine()
    {
        chainImage.material
            .DOOffset(new Vector2(0, offset), duration)
            .From(Vector2.zero)
            .SetEase(animCurve);

        uiRectTransform
            .DOAnchorPos(Vector2.zero, duration)
            .From(new Vector2(0f, (1080 / 2 * -1) - 300f))
            .SetEase(animCurve);

        yield return new WaitForSeconds(.5f);

        _audioSource.Play();
    }
}
