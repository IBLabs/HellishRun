using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MSTabletopSpinner : MonoBehaviour
{
    private void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 10f, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetEase(Ease.Linear)
            .From(Vector3.zero);

        transform.DOMoveY(.3f, 5f).From(0f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
    }
}
