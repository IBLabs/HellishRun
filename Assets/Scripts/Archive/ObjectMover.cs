using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float speed = 20f;
    public float destroyDelay = 3f;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void Update()
    {
        transform.position += Vector3.left * (speed * Time.deltaTime);
    }
}
