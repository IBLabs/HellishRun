using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeatInstructor : MonoBehaviour
{
    private const string TAG = "[BEAT INSTRUCTOR]";

    public static event Action<String> ShowBeat;
    public static event Action HideBeats;
    public static event Action<List<String>> GeneratedNewSequence;
    public static event Action<List<String>> FinishedSequence;
    
    private List<string> notes;
    private int index;
    private int sequenceLength;
    private bool isActive;

    private string[] possibleNotes = { "left", "right", "jump" };

    void Start()
    {
        sequenceLength = 3;
        GenerateSequence();
    }

    void GenerateSequence()
    {
        notes = new List<string>();

        for (int i = 0; i < sequenceLength; i++)
        {
            int randomIndex = Random.Range(0, possibleNotes.Length);
            notes.Add(possibleNotes[randomIndex]);
        }
        
        GeneratedNewSequence?.Invoke(notes);
    }

    public void ShowSequence()
    {
        index = 0;
        isActive = true;
        Debug.Log($"{TAG}: New sequence:");
    }

    public void BeatHit()
    {
        if (!isActive) return; 

        if (index < notes.Count)
        {
            Debug.Log($"{TAG}: {notes[index]}");
            ShowBeat?.Invoke(notes[index]);
            index++;
        } 
        else if (index == notes.Count)
        {
            HideBeats?.Invoke();
            index++;
        }
        else
        {
            Debug.Log($"{TAG}: Done!");

            FinishedSequence?.Invoke(notes);
                
            isActive = false;
            sequenceLength++;
            GenerateSequence();
        }
    }
}