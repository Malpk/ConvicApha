using UnityEngine;
using UnityEngine.Events;

public class DeviceSwitcher : MonoBehaviour
{
    [SerializeField] private bool _playOnStart;
    [Min(0)]
    [SerializeField] private float _timeActive = 1f;
    [Min(0)]
    [SerializeField] private float _timeDeactive = 1f;
    [Header("Events")]
    [SerializeField] private UnityEvent<bool> OnUpdateMode;

    private float _progress = 0f;
    private System.Action State;

    private void OnValidate()
    {
        SetMode(_playOnStart);
        enabled = _playOnStart;
        if (_playOnStart)
            State = ActiveState;
        else
            State = DeactiveState;
    }

    private void Update()
    {
        State();
    }

    public void Play()
    {
        enabled = true;
        State = DeactiveState;
        SetMode(false);
    }

    public void Stop()
    {
        enabled = false;
        SetMode(false);
    }

    private void DeactiveState()
    {
        _progress += Time.deltaTime / _timeDeactive;
        if (_progress >= 1f)
        {
            State = ActiveState;
            SetMode(true);
        }
    }
    private void ActiveState()
    {
        _progress += Time.deltaTime / _timeActive;
        if (_progress >= 1f)
        {
            State = DeactiveState;
            SetMode(false);
        }
    }

    private void SetMode(bool mode)
    {
        _progress = 0f;
        OnUpdateMode.Invoke(mode);
    }

}
