using System;
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

        [SerializeField] private TextMeshProUGUI healthText;
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

            healthText.text = $"{healthCount}";

            if (healthCount == 0)
            {
                PlayerLose?.Invoke();
            }
        }
    }
}