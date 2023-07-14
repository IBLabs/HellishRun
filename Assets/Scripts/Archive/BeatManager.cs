using System;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    public static event Action BeatHit;
    
    [SerializeField] private float _bpm;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Interval[] _intervals;

    private void Update()
    {
        foreach (Interval interval in _intervals)
        {
            float sampledTime = _audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetBeatLength(_bpm));
            bool didBeatHit = interval.CheckForNewInterval(sampledTime);
            if (didBeatHit) BeatHit?.Invoke();
        }
    }
}

[System.Serializable]
public class Interval
{
    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;
    private int _lastInterval;

    public float GetBeatLength(float bpm)
    {
        return 60f / (bpm * _steps);
    }

    public bool CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();

            return true;
        }

        return false;
    }
}