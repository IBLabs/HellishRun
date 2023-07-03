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
        Debug.Log("[TEST] exit trigger detected!");
        
        int successHitLayerMask = LayerMask.GetMask("SuccessHit");

        if ((successHitLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            audioSource.PlayOneShot(successHitClip);
            PerformedSuccessHit?.Invoke();
        }
    }
}
