using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string TAG = "[GAME MANAGER]";
    
    [SerializeField] private BeatInstructor _beatInstructor;
    [SerializeField] private MSObstacleTrackController _obstacleTrackController;
    
    public GameState CurrentState => _currentState;

    private GameState _currentState = GameState.Idle;

    private void Start()
    {
        UpdateState(
            new GameStateChangeDescriptor()
            {
                newState = GameState.ShowingBeats
            }
        );
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        BeatInstructor.FinishedSequence += OnFinishedSequence;
        MSObstacleTrackController.FinishedSpawningObstacles += OnFinishedSpawningObstacles;
    }

    private void UnsubscribeEvents()
    {
        BeatInstructor.FinishedSequence -= OnFinishedSequence;
        MSObstacleTrackController.FinishedSpawningObstacles -= OnFinishedSpawningObstacles;
    }

    private void UpdateState(GameStateChangeDescriptor descriptor)
    {
        if (descriptor.newState == _currentState)
        {
            Debug.Log($"{TAG}: received same state, aborting state update");
            return;
        }
        
        _currentState = descriptor.newState;

        switch (descriptor.newState)
        {
            case GameState.ShowingBeats:
                _beatInstructor.ShowSequence();
                break;
            
            case GameState.Obstacles:
                _obstacleTrackController.StartSpawning((List<string>)descriptor.context["obstacles"]);
                break;

            case GameState.BeatSuccess:
                UpdateState(new GameStateChangeDescriptor()
                {
                    newState = GameState.ShowingBeats
                });
                break;
        }
    }

    private void OnFinishedSequence(List<string> notes)
    {
        GameStateChangeDescriptor descriptor = new GameStateChangeDescriptor
        {
            newState = GameState.Obstacles,
            context =
            {
                ["obstacles"] = notes
            }
        };
        
        Debug.Log($"{TAG}: changing state to OBSTACLES");
        UpdateState(descriptor);
    }

    private void OnFinishedSpawningObstacles()
    {
        GameStateChangeDescriptor descriptor = new GameStateChangeDescriptor
        {
            newState = GameState.BeatSuccess
        };
        
        Debug.Log($"{TAG}: changing state to BEAT SUCCESS");
        UpdateState(descriptor);
    }
}

public enum GameState
{
    Idle, ShowingBeats, Obstacles, BeatSuccess, BeatLose
}

public class GameStateChangeDescriptor
{
    public GameState newState;
    public Dictionary<string, object> context = new Dictionary<string, object>();
}