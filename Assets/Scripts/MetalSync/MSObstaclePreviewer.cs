using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MSObstaclePreviewer : MonoBehaviour
{
    public GameObject obstacleIconPrefab; // Prefab for the obstacle icon
    public float padding = 10f; // Padding between icons
    public AnimationCurve entryCurve;

    private RectTransform panelRectTransform;
    private Queue<GameObject> icons = new Queue<GameObject>(); // Queue to hold the obstacle icons
    private Queue<GameObject> iconsToKill = new Queue<GameObject>();

    private Vector2 animationStartPos;
    private Vector2 animationEndPos;
    private int currentIconIndex;

    // Event to signal that the animation has finished
    public static event Action AnimationFinished = delegate { };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowObstacle("testObstacle");
        }
    }

    private void OnEnable()
    {
        panelRectTransform = GetComponent<RectTransform>();
        
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        BeatInstructor.ShowBeat += ShowObstacle;
        BeatInstructor.GeneratedNewSequence += OnGeneratedNewSequence;
        BeatInstructor.HideBeats += OnHideBeats;
    }

    private void UnsubscribeEvents()
    {
        BeatInstructor.ShowBeat -= ShowObstacle;
        BeatInstructor.GeneratedNewSequence -= OnGeneratedNewSequence;
        BeatInstructor.HideBeats -= OnHideBeats;
    }

    public void ShowObstacle(string obstacleIdentifier)
    {
        var icon = icons.ToArray()[currentIconIndex];
        
        icon.SetActive(true);
            
        // Move the icon from the start position to the end position
        StartCoroutine(MoveIcon(icon.GetComponent<RectTransform>(), animationStartPos, animationEndPos));
        animationEndPos += Vector2.right * padding;

        currentIconIndex++;
        
        iconsToKill.Enqueue(icon);

        if (currentIconIndex == icons.Count)
        {
            StartCoroutine(KillOldIcons(iconsToKill, 1.5f));
        }
    }

    private void OnGeneratedNewSequence(List<String> notes)
    {
        currentIconIndex = 0;
        icons.Clear();

        for (int i = 0; i < notes.Count; i++)
        {
            // Instantiate a new obstacle icon as a child of the panel
            GameObject icon = Instantiate(obstacleIconPrefab, panelRectTransform);
            icon.SetActive(false);

            // Get the ObstacleIcon component and set the identifier
            // ObstacleIcon obstacleIcon = icon.GetComponent<ObstacleIcon>();
            // obstacleIcon.SetIdentifier(obstacleIdentifier);

            // Enqueue the icon
            icons.Enqueue(icon);
        }
        
        // Calculate the start and end positions for the icons
        animationStartPos = new Vector2(panelRectTransform.rect.width, 0);
        animationEndPos = Vector2.zero - new Vector2(((icons.Count - 1f) * padding) / 2f, 0);
    }

    private void OnHideBeats()
    {
        KillOldIcons(iconsToKill, 0f);
    }

    private IEnumerator KillOldIcons(Queue<GameObject> icons, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var icon in icons)
        {
            StartCoroutine(FadeOutIcon(icon.GetComponent<Image>()));
        }

        yield return new WaitForSeconds(2f);

        foreach (var icon in icons)
        {
            Destroy(icon.gameObject);
        }
        
        iconsToKill.Clear();
    }

    private IEnumerator MoveIcon(RectTransform icon, Vector2 startPos, Vector2 endPos)
    {
        float duration = .3f; // The duration of the movement animation
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = entryCurve.Evaluate(elapsed / duration);
            icon.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

    private IEnumerator FadeOutIcon(Image icon)
    {
        float duration = .2f;
        float elapsed = 0f;

        Color startColor = icon.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            icon.color = Color.Lerp(startColor, startColor.WithAlpha(0), elapsed / duration);
            yield return null;
        }
    }
}
