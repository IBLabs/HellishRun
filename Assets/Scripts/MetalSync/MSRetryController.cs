using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSRetryController : MonoBehaviour
{
    [SerializeField] private KeyCode retryKey;

    [SerializeField] private MSScriptTransitionController transitionController;
    [SerializeField] private AudioSource audioSource;

    public void Retry()
    {
        StartCoroutine(RetryCoroutine());
    }

    private IEnumerator RetryCoroutine()
    {
        transitionController.PerformFadeOut();
        audioSource.DOFade(0, 1f);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(1);
    }
}
