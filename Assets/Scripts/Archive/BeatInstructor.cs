using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeatInstructor : MonoBehaviour
{
    private const string TAG = "[BEAT INSTRUCTOR]";

    public static event Action<ObstacleNote> ShowBeat;
    public static event Action HideBeats;
    public static event Action<List<ObstacleNote>> GeneratedNewSequence;
    public static event Action<List<ObstacleNote>> FinishedSequence;

    [SerializeField] private int maximumSequenceLength = 6;
    [SerializeField] private int minimumBeatCount = 1;
    [SerializeField] private int maximumBeatCount = 5;

    private List<ObstacleNote> notes;
    private int index;
    private int sequenceLength;
    private bool isActive;

    private string[] possibleNotes = { "left", "right", "jump" };

    private int currentNoteBeatCounter;

    void Start()
    {
        sequenceLength = 3;
        GenerateSequence();
    }

    void GenerateSequence()
    {
        notes = new List<ObstacleNote>();

        for (int i = 0; i < sequenceLength; i++)
        {
            int randomIndex = Random.Range(0, possibleNotes.Length);
            string noteIdentifier = possibleNotes[randomIndex];
            int beatCount = Random.Range(minimumBeatCount, maximumBeatCount);
            ObstacleNote newNote = new ObstacleNote(noteIdentifier, beatCount); 
            notes.Add(newNote);
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
            ObstacleNote currentNote = notes[index];

            if (currentNoteBeatCounter < currentNote.beatCount - 1)
            {
                currentNoteBeatCounter++;
                return;
            }
            
            Debug.Log($"{TAG}: {currentNote}");
            ShowBeat?.Invoke(notes[index]);

            currentNoteBeatCounter = 0;
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
            sequenceLength = Math.Min(sequenceLength + 1, maximumSequenceLength); 
            GenerateSequence();
        }
    }
}

[Serializable]
public class ObstacleNote
{
    public string identifier;
    public int beatCount;

    public ObstacleNote(string identifier, int beatCount)
    {
        this.identifier = identifier;
        this.beatCount = beatCount;
    }
}