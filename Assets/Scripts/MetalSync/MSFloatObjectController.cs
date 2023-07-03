using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MSFloatObjectController : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoopFloatCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LoopFloatCoroutine()
    {
        float duration = 2f + Random.Range(1f, 2.5f);
        float timer = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.down;

        while (true)
        {
            float targetT = curve.Evaluate(timer / duration);
            transform.position = Vector3.Lerp(startPos, endPos, targetT);
            
            timer += Time.deltaTime;
            if (timer > duration)
            {
                timer = 0;
            }
            yield return null;
        }
    }
}
