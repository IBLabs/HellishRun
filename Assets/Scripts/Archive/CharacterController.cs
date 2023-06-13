using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Animator animator;
    
    private static readonly int Jump = Animator.StringToHash("Jump");

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger(Jump);
        }
    }
}
