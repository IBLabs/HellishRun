using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MSObstacleTrackController : MonoBehaviour
{
    public static event Action FinishedSpawningObstacles;

    public GameObject obstaclePrefab; // The prefab for the obstacles
    public float speed = 5.0f; // The speed at which the obstacles move
    public float laneWidth = 3.0f; // The width of a lane
    public int numLanes = 3;

    private List<GameObject> activeObstacles; // The list of currently active obstacles
    
    private List<string> sequence;
    private int index;

    private void Start()
    {
        // Initialize the activeObstacles list
        activeObstacles = new List<GameObject>();
    }

    private void Update()
    {
        // Move the obstacles
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = activeObstacles[i];
            obstacle.transform.position -= Vector3.forward * (speed * Time.deltaTime);

            // If the obstacle has moved past the player, remove it
            if (obstacle.transform.position.z <= -(laneWidth * 4))
            {
                Destroy(obstacle);
                activeObstacles.RemoveAt(i);
            }
        }
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

    private void SpawnObstacle(string obstacleIdentifier)
    {
        var lane = 0f;

        // Calculate the x position based on the lane
        float xPos = lane * laneWidth;

        // Create a new obstacle at the specified position
        GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(xPos, transform.position.y, transform.position.z), Quaternion.identity, transform);
        activeObstacles.Add(obstacle);
    }
}