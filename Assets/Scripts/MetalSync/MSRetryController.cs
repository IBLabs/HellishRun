using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSRetryController : MonoBehaviour
{
    [SerializeField] private KeyCode retryKey;

    [SerializeField] private MSScriptTransitionController transitionController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(retryKey))
        {
            StartCoroutine(RetryCoroutine());
        }
    }

    private IEnumerator RetryCoroutine()
    {
        transitionController.PerformFadeOut();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(1);
    }
}
