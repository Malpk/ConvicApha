using UnityEngine;

public abstract class TaskState 
{
    private float _timeActive;

    protected float progress { get; private set; } = 0f;

    public void Start(float timeActive)
    {
        progress = 0f;
        _timeActive = timeActive;
    }

    public bool Update()
    {
        progress += Time.deltaTime/_timeActive;
        UpadateTask();
        return progress < 1f;
    }

    protected abstract void UpadateTask();
}
