using UnityEngine;

public class TpStartPoint : MonoBehaviour
{
    public event System.Action OnComplite;

    public void CompliteAnimatuinEvenet()
    {
        OnComplite?.Invoke();
    }
}
