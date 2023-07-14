using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace MetalSync
{
    public class MSPlayerLoseController : MonoBehaviour
    {
        [SerializeField] private MSTransitionController transitionController;
        [SerializeField] private AudioSource bgmAudioSource;

        #region Event Handling
        
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
            MSHealthBarController.PlayerLose += OnPlayerLose;
        }

        private void Unsubscribe()
        {
            MSHealthBarController.PlayerLose -= OnPlayerLose;
        }

        private void OnPlayerLose()
        {
            StartCoroutine(LoseCoroutine());
        }
        
        #endregion

        private IEnumerator LoseCoroutine()
        {
            const float duration = 1f;
            
            DOTween.To(() => Time.timeScale, x => Time.timeScale = (float)x, .2, duration);
            bgmAudioSource.DOPitch(.5f, duration);
            transitionController.FadeIn();
            
            yield return new WaitForSeconds(duration / 2);

            bgmAudioSource.DOFade(0f, .2f);

            // TODO: show lose screen
        }
    }
}