using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MSHudController : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    
    void Start()
    {
        PerformEnterAnimation();   
    }

    private void PerformEnterAnimation()
    {
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        float positionY = anchoredPos.y;
        rectTransform.DOAnchorPosY(positionY, 1f).From(new Vector2(anchoredPos.x, positionY - 500f)).SetEase(Ease.OutBack);
    }
}
