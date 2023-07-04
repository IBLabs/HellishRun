using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MetalSync
{
    public class MSEndlessObstaclePreviewer : MonoBehaviour
    {
        [SerializeField] private Transform parentCanvas;
        [SerializeField] private GameObject iconTemplatePrefab;
        
        [SerializeField] private float iconSize = 56f;
        [SerializeField] private float padding = 16f;
        [SerializeField] private int maxIconCount = 7;

        private Queue<GameObject> activeIcons = new();

        private void Start()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SpawnIcon();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                RemoveIcon();
            }
        }

        private void SpawnIcon()
        {
            if (activeIcons.Count == maxIconCount) return;

            GameObject newIcon = CreateNewIcon(iconSize);

            RectTransform newIconRectTransform = newIcon.GetComponent<RectTransform>();
            float xPos = CalculateIconXPosition(activeIcons.Count);
            newIconRectTransform.anchoredPosition = new Vector2(xPos, 0f);

            MSIntructionsAnimator instructionAnimator = newIcon.GetComponent<MSIntructionsAnimator>();
            if (instructionAnimator != null)
            {
                instructionAnimator.SetVisible(true, false);
            }

            activeIcons.Enqueue(newIcon);
        }

        private void RemoveIcon()
        {
            GameObject firstActiveIcon = activeIcons.Dequeue();

            MSIntructionsAnimator instructionAnimator = firstActiveIcon.GetComponent<MSIntructionsAnimator>();
            if (instructionAnimator != null)
            {
                instructionAnimator.SetVisible(false, true);    
            }
            else
            {
                Destroy(firstActiveIcon);
            }

            GameObject[] activeIconsArr = activeIcons.ToArray();
            for (int i = 0; i < activeIconsArr.Length; i ++)
            {
                GameObject icon = activeIconsArr[i];
                RectTransform iconRectTransform = icon.GetComponent<RectTransform>();
                Vector2 newAnchoredPos = new Vector2(
                    CalculateIconXPosition(i), 
                    iconRectTransform.anchoredPosition.y
                );

                iconRectTransform.DOAnchorPos(newAnchoredPos, .2f).SetDelay(i * .05f);
            }
        }

        private GameObject CreateNewIcon(float iconSize)
        {
            GameObject newIcon;
            
            if (iconTemplatePrefab != null)
            {
                newIcon = Instantiate(iconTemplatePrefab, parentCanvas.transform);
            }
            else
            {
                newIcon = new GameObject("Icon");
                newIcon.transform.parent = parentCanvas;
            
                newIcon.AddComponent<Image>().color = Color.yellow;
            }

            Image newIconImage = newIcon.GetComponent<Image>();
            newIconImage.material = new Material(newIconImage.material);
            
            RectTransform newIconRectTransform = newIcon.GetComponent<RectTransform>();
            newIconRectTransform.sizeDelta = new Vector2(iconSize, iconSize);
            newIconRectTransform.anchorMin = new Vector2(.5f, .5f);
            newIconRectTransform.anchorMax = new Vector2(.5f, .5f);
            newIconRectTransform.pivot = new Vector2(.5f, .5f);

            return newIcon;
        }

        private float CalculateIconXPosition(int iconIndex)
        {
            if (iconIndex >= maxIconCount) return -1;

            return -((maxIconCount - 1) * iconSize + (maxIconCount - 1) * padding) / 2 +
                   iconIndex * (iconSize + padding);
        }
    }
}