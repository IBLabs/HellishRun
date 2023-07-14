using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MetalSync
{
    public class MSBeatObstacleGenerator : MonoBehaviour
    {
        public static event Action<MSSimpleObstacleNote> SpawnObstacle;

        [SerializeField] private MSEndlessObstaclePreviewer endlessPreviewer;

        [SerializeField] private int minimumBeatCount = 1;
        [SerializeField] private int maximumBeatCount = 3;

        public bool isActive;

        private int nextBeatCount;
        private int beatCounter;

        private string[] possibleNotes = { "left", "right", "jump" };
        

        private void Awake()
        {
            nextBeatCount = GenerateNextBeatCount();
        }

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
            BeatManager.BeatHit += OnBeatHit;
        }

        private void Unsubscribe()
        {
            BeatManager.BeatHit -= OnBeatHit;
        }

        private void OnBeatHit()
        {
            if (!isActive) return;

            beatCounter++;

            if (beatCounter < nextBeatCount) return;

            string targetNoteIdentifier = possibleNotes[Random.Range(0, possibleNotes.Length)];
            MSSimpleObstacleNote newNote = new MSSimpleObstacleNote(targetNoteIdentifier);
            
            SpawnObstacle?.Invoke(newNote);

            beatCounter = 0;
            nextBeatCount = GenerateNextBeatCount();
        }

        private int GenerateNextBeatCount()
        {
            return Random.Range(minimumBeatCount, maximumBeatCount);
        }
    }
}