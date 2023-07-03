using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MSIntroLightningsController : MonoBehaviour
{
    [SerializeField] private List<VisualEffect> lightnings;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip thunderClip;

    [SerializeField] private float lightningFXDelay = .8f;
    [SerializeField] private float minDelay = 1f;
    [SerializeField] private float maxDelay = 2f;

    private float nextLightningTime;
    
    void Update()
    {
        if (Time.time > nextLightningTime)
        {
            nextLightningTime = Time.time + Random.Range(minDelay, maxDelay);
            PlayLightning();
        }
    }

    private void PlayLightning()
    {
        VisualEffect randomLightning = lightnings[Random.Range(0, lightnings.Count)];
        StartCoroutine(lightningCoroutine(randomLightning));
    }

    private IEnumerator lightningCoroutine(VisualEffect target)
    {
        _audioSource.PlayOneShot(thunderClip);

        yield return new WaitForSeconds(lightningFXDelay);
        
        target.Play();
    }
}
