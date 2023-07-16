using System;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MetalSync
{
    public class MSHealthBarController : MonoBehaviour
    {
        public static event Action PlayerLose;

        [SerializeField] private List<Image> heartImages;
        [SerializeField] private Image redScreenImage;

        [SerializeField] private Color initialColor;

        private int healthCount = 3;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            MSCharacterController.PlayerTookHit += OnPlayerHit;
        }

        private void Unsubscribe()
        {
            MSCharacterController.PlayerTookHit -= OnPlayerHit;
        }

        private void OnPlayerHit()
        {
            redScreenImage.DOColor(Color.black.WithAlpha(0), .5f).From(initialColor);

            healthCount -= 1;


            heartImages[healthCount].gameObject.SetActive(false);

            if (healthCount == 0)
            {
                PlayerLose?.Invoke();
            }
        }
    }
}