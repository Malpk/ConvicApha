using UnityEngine;
using UnityEngine.Events;

public class DeathTimer : MonoBehaviour
{
    [SerializeField] private bool _playOnStart;
    [Min(1)]
    [SerializeField] private float _timeActive = 1f;
    [Header("Events")]
    [SerializeField] private UnityEvent _onCompliteWork;

    private float _progress;

    private void Start()
    {
        if (_playOnStart)
            Play();
        else
            Stop();
    }

    private void Update()
    {
        _progress += Time.deltaTime / _timeActive;
        if (_progress >= 1f)
        {
            _onCompliteWork.Invoke();
            Stop();
        }
    }

    public void Play()
    {
        _progress = 0f;
        enabled = true;
    }
    public void Stop()
    {
        enabled = false;
    }
}
