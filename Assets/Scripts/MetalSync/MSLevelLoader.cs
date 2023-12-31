using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MSLevelLoader : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource clipAudioSource;
    [SerializeField] private AudioClip startGameClip;

    public void LoadNextScene(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PerformStartGameMusic();
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        _animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(2.5f);
        
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    private void PerformStartGameMusic()
    {
        bgmAudioSource.DOFade(0f, .3f);
        clipAudioSource.PlayOneShot(startGameClip);
    }
}
