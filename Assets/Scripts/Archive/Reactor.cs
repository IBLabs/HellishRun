using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Reactor : MonoBehaviour
{
    public float returnSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * returnSpeed);
    }

    public void ScaleUp()
    {
        transform.localScale = Vector3.one * 1.2f;
    }
}
