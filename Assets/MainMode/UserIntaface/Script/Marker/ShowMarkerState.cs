using UnityEngine;

public class ShowMarkerState : TaskState
{
    private readonly Transform marker;

    public ShowMarkerState(Transform marker)
    {
        this.marker = marker;
    }
    protected override void UpadateTask()
    {
        marker.gameObject.SetActive(progress < 1f);
    }
}
