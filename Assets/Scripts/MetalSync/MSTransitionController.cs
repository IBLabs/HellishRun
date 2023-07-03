using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSTransitionController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        _animator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        _animator.SetTrigger("FadeOut");
    }
}
