using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpreader : MonoBehaviour
{
    private const string TAG = "[OBJECT SPREADER]";
    
    public static event Action FinishedSpawningObstacles;

    public List<GameObject> obstacles;

    private List<string> sequence;
    private int index;

    void Start()
    {
        sequence = new List<string>();
    }

    public void StartSpawning(List<string> newSequence)
    {
        // Set the new sequence and reset the index
        sequence = newSequence;
        index = 0;
    }

    public void BeatHit()
    {
        if (index >= sequence.Count) return;

        // Spawn an obstacle based on the current note
        SpawnObstacle(sequence[index]);
        index++;

        if (index == sequence.Count)
            FinishedSpawningObstacles?.Invoke();
    }

    void SpawnObstacle(string note)
    {
        Debug.Log($"{TAG}: spawned obstacle!");
        Instantiate(obstacles[Random.Range(0, obstacles.Count)], new Vector3(30f, transform.position.y,transform.position.z), Quaternion.identity);
    }
}
