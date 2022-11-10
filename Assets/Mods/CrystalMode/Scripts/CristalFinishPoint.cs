using UnityEngine;
using UnityEngine.Events;

public class CristalFinishPoint : MonoBehaviour
{
    public UnityEvent _complite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _complite.Invoke();
        }
    }
}
