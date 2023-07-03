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
    public float padding = .5f;
    public List<ObstacleType> obstaclePrefabs;

    private List<GameObject> activeObstacles; // The list of currently active obstacles

    private List<ObstacleNote> sequence;
    private int index;

    private int currentNoteBeatCounter;

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
    
    public void StartSpawning(List<ObstacleNote> newSequence)
    {
        // Set the new sequence and reset the index
        sequence = newSequence;
        index = 0;
    }
    
    public void BeatHit()
    {
        if (sequence == null || index >= sequence.Count) return;

        // Spawn an obstacle based on the current note
        ObstacleNote currentNote = sequence[index];

        if (currentNoteBeatCounter < currentNote.beatCount - 1)
        {
            currentNoteBeatCounter++;
            return;
        }
        
        SpawnObstacle(currentNote);

        currentNoteBeatCounter = 0;
        index++;

        if (index == sequence.Count)
            FinishedSpawningObstacles?.Invoke();
    }

    private void SpawnObstacle(ObstacleNote note)
    {
        ObstaclePositionType posType;
        if (!Enum.TryParse(note.identifier, true, out posType))
        {
            Debug.Log("[TEST]: obstacle position type is unsupported");
            return;
        }
        
        float obstacleX = calculateObstacleX(posType);
            
        // Create a new obstacle at the specified position
        GameObject targetPrefab = getObstaclePrefab(posType);
        GameObject obstacle = Instantiate(targetPrefab, new Vector3(obstacleX, transform.position.y, transform.position.z), Quaternion.identity, transform);
        activeObstacles.Add(obstacle);
    }

    private float calculateObstacleX(ObstaclePositionType posType)
    {
        float trackWidth = (numLanes * laneWidth) - (padding * 2);
        switch (posType)
        {
            // case ObstaclePositionType.Left:
            //     return trackWidth * -.5f;
            //
            // case ObstaclePositionType.Right:
            //     return trackWidth * .5f;
            //
            // case ObstaclePositionType.Jump:
            //     return 0f;
            
            default:
                Debug.Log("[TEST]: failed to calculate obstacle X, unsupported position type");
                return 0f;
        }
    }

    private GameObject getObstaclePrefab(ObstaclePositionType posType)
    {
        return obstaclePrefabs.Find(obsType => obsType.identifier == posType).prefab;
    }
}

[Serializable]
public class ObstacleType
{
    public ObstaclePositionType identifier;
    public GameObject prefab;
}

[Serializable]
public enum ObstaclePositionType
{
    Left,
    Right,
    Jump
}