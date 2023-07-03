using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class MSObstaclePreviewer : MonoBehaviour
{
    public static event Action PlayerFinishedObstacleSequence;
    
    public GameObject obstacleIconPrefab; // Prefab for the obstacle icon
    public float padding = 10f; // Padding between icons
    public float spawnYPosition = 0f;

    private RectTransform panelRectTransform;
    private Queue<GameObject> icons = new(); // Queue to hold the obstacle icons
    private Queue<GameObject> iconsToKill = new();

    private Vector2 animationEndPos;
    private int currentIconIndex;
    
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
        
        MSObstacleLighter.SuccessHit += OnSuccessHit;
    }

    private void UnsubscribeEvents()
    {
        BeatInstructor.ShowBeat -= ShowObstacle;
        BeatInstructor.GeneratedNewSequence -= OnGeneratedNewSequence;
        BeatInstructor.HideBeats -= OnHideBeats;

        MSObstacleLighter.SuccessHit -= OnSuccessHit;
    }

    public void ShowObstacle(ObstacleNote obstacleNote)
    {
        if (icons.Count == 0) return;

        var icon = icons.ToArray()[currentIconIndex];
        
        icon.SetActive(true);
            
        // Move the icon from the start position to the end position
        icon.GetComponent<RectTransform>().anchoredPosition = animationEndPos;
        icon.GetComponent<MSIntructionsAnimator>().SetVisible(true, false);
        
        animationEndPos += Vector2.right * padding;

        currentIconIndex++;
        
        iconsToKill.Enqueue(icon);

        /*
        if (currentIconIndex == icons.Count)
        {
            StartCoroutine(KillOldIcons(iconsToKill, 1.5f));
        }
        */
    }

    private void OnGeneratedNewSequence(List<ObstacleNote> notes)
    {
        currentIconIndex = 0;
        icons.Clear();

        for (int i = 0; i < notes.Count; i++)
        {
            GameObject icon = Instantiate(obstacleIconPrefab, panelRectTransform);
            icon.SetActive(false);
            
            Material updatedMat = new Material(icon.GetComponent<Image>().material);
            updatedMat.SetTexture("_Texture", Resources.Load<Texture>(notes[i].identifier));
            icon.GetComponent<Image>().material = updatedMat;

            icons.Enqueue(icon);
        }
        
        animationEndPos = Vector2.zero - new Vector2(((icons.Count - 1f) * padding) / 2f, spawnYPosition);
    }

    private void OnHideBeats()
    {
        KillOldIcons(iconsToKill, 0f);
    }

    private IEnumerator KillOldIcons(Queue<GameObject> icons, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var icon in icons)
            icon.GetComponent<MSIntructionsAnimator>().SetVisible(false, true);

        yield return new WaitForSeconds(2f);

        foreach (var icon in icons)
            Destroy(icon.gameObject);
        
        iconsToKill.Clear();
    }

    private void OnSuccessHit()
    {
        var currentIcon = iconsToKill.Dequeue();
        currentIcon.GetComponent<MSIntructionsAnimator>().SetVisible(false, true);
        
        // Destroy(currentIcon);

        if (iconsToKill.Count == 0) PlayerFinishedObstacleSequence?.Invoke();
    }
}
