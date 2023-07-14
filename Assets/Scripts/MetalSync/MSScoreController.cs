using TMPro;
using UnityEngine;

namespace MetalSync
{
    public class MSScoreController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private int comboCount;

        #region EventHandling
        
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
            MSObstacleLighter.SuccessHit += OnSuccessHit;
            MSCharacterController.PlayerTookHit += OnPlayerHit;
        }

        private void Unsubscribe()
        {
            MSObstacleLighter.SuccessHit -= OnSuccessHit;
            MSCharacterController.PlayerTookHit -= OnPlayerHit;
        }

        private void OnSuccessHit()
        {
            comboCount += 1;
            scoreText.text = $"{comboCount}";
        }

        private void OnPlayerHit()
        {
            comboCount = 0;
            scoreText.text = $"{comboCount}";
        }
        
        #endregion
    }
}