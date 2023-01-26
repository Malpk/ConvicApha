using UnityEngine;
using UnityEngine.Events;

public class Device : MonoBehaviour
{
    [SerializeField] private bool _playOnStart;
    [Header("Events")]
    [SerializeField] private UnityEvent _onPlay;
    [SerializeField] private UnityEvent _onStop;

    private void Start()
    {
        if (_playOnStart)
            Play();
        else
            Stop();
    }

    public void Play()
    {
        _onPlay.Invoke();
    }
    public void Stop()
    {
        _onStop.Invoke();
    }
}
