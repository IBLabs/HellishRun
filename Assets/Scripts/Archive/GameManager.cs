using System;
using System.Collections;
using System.Collections.Generic;
using MetalSync;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string TAG = "[GAME MANAGER]";
    
    [SerializeField] private BeatInstructor _beatInstructor;
    [SerializeField] private MSObstacleTrackController _obstacleTrackController;

    [SerializeField] private MSBeatObstacleGenerator _beatObstacleGenerator;

    [SerializeField] private MSTutorialController _tutorialController;
    
    public GameState CurrentState => _currentState;

    private GameState _currentState = GameState.Idle;

    private void Start()
    {
        StartCoroutine(PlayIntroCoroutine());
        UpdateState(
            new GameStateChangeDescriptor()
            {
                newState = GameState.Intro
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
        MSObstaclePreviewer.PlayerFinishedObstacleSequence += OnPlayerFinishedObstacleSequence;
        MSTutorialController.TutorialFinished += OnTutorialFinished;
    }

    private void UnsubscribeEvents()
    {
        BeatInstructor.FinishedSequence -= OnFinishedSequence;
        MSObstacleTrackController.FinishedSpawningObstacles -= OnFinishedSpawningObstacles;
        MSObstaclePreviewer.PlayerFinishedObstacleSequence -= OnPlayerFinishedObstacleSequence;
        MSTutorialController.TutorialFinished -= OnTutorialFinished;
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
            case GameState.Intro:
                break;
            
            case GameState.Tutorial :
                _tutorialController.StartTutorial();
                break;
            
            case GameState.EndlessBeats:
                _beatObstacleGenerator.isActive = true;
                break;

            case GameState.ShowingBeats:
                _beatInstructor.ShowSequence();
                break;
            
            case GameState.Obstacles:
                _obstacleTrackController.StartSpawning((List<MSObstacleNote>)descriptor.context["obstacles"]);
                break;
            
            case GameState.PlayerTurn:
                break;

            case GameState.BeatSuccess:
                UpdateState(new GameStateChangeDescriptor()
                {
                    newState = GameState.ShowingBeats
                });
                break;
        }
    }

    private void OnFinishedSequence(List<MSObstacleNote> notes)
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
            newState = GameState.PlayerTurn
        };
        
        Debug.Log($"{TAG}: changing state to PLAYER TURN");
        UpdateState(descriptor);
    }

    private void OnPlayerFinishedObstacleSequence()
    {
        GameStateChangeDescriptor descriptor = new GameStateChangeDescriptor
        {
            newState = GameState.BeatSuccess
        };
        
        Debug.Log($"{TAG}: changing state to OBSTACLE SUCCESS");
        UpdateState(descriptor);
    }

    private void OnTutorialFinished()
    {
        GameStateChangeDescriptor descriptor = new GameStateChangeDescriptor
        {
            newState = GameState.EndlessBeats
        };
        
        Debug.Log($"{TAG}: changine state to TUTORIAL");
        UpdateState(descriptor);
    }

    private IEnumerator PlayIntroCoroutine()
    {
        yield return new WaitForSeconds(5);
        UpdateState(new GameStateChangeDescriptor()
        {
            newState = GameState.Tutorial
        });
    }
}

public enum GameState
{
    Idle, Intro, Tutorial, EndlessBeats, ShowingBeats, Obstacles, BeatSuccess, PlayerTurn, BeatLose
}

public class GameStateChangeDescriptor
{
    public GameState newState;
    public Dictionary<string, object> context = new Dictionary<string, object>();
}