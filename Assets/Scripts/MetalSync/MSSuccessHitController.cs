using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MSSuccessHitController : MonoBehaviour
{
    public static event Action PerformedSuccessHit;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successHitClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SuccessHit"))
        {
            audioSource.PlayOneShot(successHitClip);
            PerformedSuccessHit?.Invoke();
        }
    }
}
