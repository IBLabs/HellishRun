using System;
using System.Collections;
using System.Collections.Generic;
using MetalSync;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MSObstacleTrackController : MonoBehaviour
{
    public static event Action FinishedSpawningObstacles;

    public float speed = 5.0f; // The speed at which the obstacles move
    public float laneWidth = 3.0f; // The width of a lane
    public int numLanes = 3;
    public float padding = .5f;
    public List<ObstacleTemplate> obstaclePrefabs;

    private List<GameObject> activeObstacles; // The list of currently active obstacles

    private List<MSObstacleNote> sequence;
    private int index;

    private int currentNoteBeatCounter;
    
    #region Event Handling

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
        MSBeatObstacleGenerator.SpawnObstacle += OnSpawnObstacle;
    }

    private void Unsubscribe()
    {
        MSBeatObstacleGenerator.SpawnObstacle -= OnSpawnObstacle;
    }

    private void OnSpawnObstacle(MSSimpleObstacleNote note)
    {
        SpawnObstacle(note.identifier);
    }
    
    #endregion

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
    
    public void StartSpawning(List<MSObstacleNote> newSequence)
    {
        // Set the new sequence and reset the index
        sequence = newSequence;
        index = 0;
    }
    
    public void BeatHit()
    {
        if (sequence == null || index >= sequence.Count) return;

        // Spawn an obstacle based on the current note
        MSObstacleNote currentNote = sequence[index];

        if (currentNoteBeatCounter < currentNote.beatCount - 1)
        {
            currentNoteBeatCounter++;
            return;
        }
        
        SpawnObstacle(currentNote.identifier);

        currentNoteBeatCounter = 0;
        index++;

        if (index == sequence.Count)
            FinishedSpawningObstacles?.Invoke();
    }

    private void SpawnObstacle(string noteIdentifier)
    {
        ObstaclePositionType posType;
        if (!Enum.TryParse(noteIdentifier, true, out posType))
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
        List<ObstacleTemplate> matching = obstaclePrefabs.FindAll(obsType => obsType.posType == posType);
        GameObject randomMatching = matching[Random.Range(0, matching.Count)].prefab;
        return randomMatching;
    }
}

[Serializable]
public class ObstacleTemplate
{
    [FormerlySerializedAs("identifier")] 
    public ObstaclePositionType posType;
    public GameObject prefab;
}

[Serializable]
public enum ObstaclePositionType
{
    Left,
    Right,
    Jump
}