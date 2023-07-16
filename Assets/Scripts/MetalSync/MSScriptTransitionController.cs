using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MSScriptTransitionController : MonoBehaviour
{
    [SerializeField] private Image transitionImage;
    [FormerlySerializedAs("light")] [SerializeField] private Light targetLight;
    
    // Start is called before the first frame update
    void Start()
    {
        PerformFadeIn();
    }

    public void PerformFadeIn()
    {
        StartCoroutine(PerformFadeInCoroutine());
    }

    private IEnumerator PerformFadeInCoroutine()
    {
        targetLight.intensity = 0f;
        
        transitionImage.DOColor(Color.black.WithAlpha(0), 1f).From(Color.black);

        yield return new WaitForSeconds(.5f);
        
        if (targetLight != null)
        {
            targetLight.DOIntensity(1f, 1f).From(0f).SetEase(Ease.Linear);
        }
    }

    public void PerformFadeOut()
    {
        transitionImage.DOColor(Color.black, 1f).From(Color.black.WithAlpha(0));
    }
}
