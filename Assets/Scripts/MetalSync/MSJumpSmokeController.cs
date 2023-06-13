using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSJumpSmokeController : MonoBehaviour
{
    private ParticleSystem jumpParticleSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        jumpParticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpParticleSystem.Play();
        }   
    }
}
