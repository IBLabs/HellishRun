using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MSObstacleLighter : MonoBehaviour
{
    public static event Action SuccessHit;
    
    [SerializeField] private Color targetColor;
    [SerializeField] private float intensity;
    [SerializeField] private float animationDuration = .5f;

    private void OnTriggerEnter(Collider other)
    {
        int successHitLayerMask = LayerMask.GetMask("Player");

        // if ((successHitLayerMask & (1 << other.gameObject.layer)) != 0)
        if (other.CompareTag("Player"))
        {
            SuccessHit?.Invoke();
            
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            StartCoroutine(FlashCoroutine(renderers));
        }
    }

    private IEnumerator FlashCoroutine(Renderer[] renderers)
    {
        float duration = animationDuration;
        float timer = 0f;

        var endColor = Color.black;
        var startColor = targetColor * intensity;

        while (timer <= duration)
        {
            foreach (var renderer in renderers)
            {
                renderer.material.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, timer / duration));
            }

            timer += Time.deltaTime;
            
            yield return null;
        }
    }
}
