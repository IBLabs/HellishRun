using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MetalSync
{
    public class MSEndlessObstaclePreviewer : MonoBehaviour
    {
        [SerializeField] private Transform parentCanvas;
        [SerializeField] private GameObject iconTemplatePrefab;

        [SerializeField] private RectTransform targetPosition;
        [SerializeField] private float iconSize = 56f;
        [SerializeField] private float padding = 16f;
        [SerializeField] private int maxIconCount = 7;

        private Queue<GameObject> activeIcons = new();
        
        #region Event Handling

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsusbsribe();
        }

        private void Subscribe()
        {
            MSBeatObstacleGenerator.SpawnObstacle += OnSpawnObstacle;
            MSObstacleLighter.SuccessHit += OnSuccessHit;
            MSCharacterController.PlayerTookHit += OnPlayerTookHit;
        }

        private void Unsusbsribe()
        {
            MSBeatObstacleGenerator.SpawnObstacle -= OnSpawnObstacle;
            MSObstacleLighter.SuccessHit -= OnSuccessHit;
            MSCharacterController.PlayerTookHit -= OnPlayerTookHit;
        }

        private void OnSpawnObstacle(MSSimpleObstacleNote note)
        {
            SpawnIcon(note);
        }

        private void OnSuccessHit()
        {
            RemoveIcon();
        }

        private void OnPlayerTookHit()
        {
            RemoveIcon();
        }
        
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SpawnIcon(new MSSimpleObstacleNote("left"));
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                RemoveIcon();
            }
        }

        private void SpawnIcon(MSSimpleObstacleNote note)
        {
            if (activeIcons.Count == maxIconCount) return;

            GameObject newIcon = CreateNewIcon(iconSize);

            Image newIconImage = newIcon.GetComponent<Image>();
            Material updatedMat = new Material(newIconImage.material);
            updatedMat.SetTexture("_Texture", Resources.Load<Texture>(note.identifier));
            newIconImage.material = updatedMat;

            RectTransform newIconRectTransform = newIcon.GetComponent<RectTransform>();
            float xPos = CalculateIconXPosition(activeIcons.Count);

            float targetY = 0;
            if (targetPosition != null) targetY = targetPosition.anchoredPosition.y;

            newIconRectTransform.anchoredPosition = new Vector2(xPos, targetY); 

            MSIntructionsAnimator instructionAnimator = newIcon.GetComponent<MSIntructionsAnimator>();
            if (instructionAnimator != null)
            {
                instructionAnimator.SetVisible(true, false);
            }

            activeIcons.Enqueue(newIcon);
        }

        private void RemoveIcon()
        {
            if (activeIcons.Count == 0) return;

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