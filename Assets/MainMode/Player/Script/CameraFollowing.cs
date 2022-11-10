using UnityEngine;
using Zenject;

public class CameraFollowing : MonoBehaviour
{
    [Header("Genral setting")]
    [Min(0)]
    [SerializeField] private float _maxOffset = 1;
    [Min(0)]
    [SerializeField] private float _timeSmooth;
    [Min(1)]
    [SerializeField] private float _speedFollowing;
    [Header("Reference")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _target;

    private Vector2 _velocity = Vector2.zero;

    public bool IsPlay { private set; get; }
    public Camera Camera => _camera;

    [Inject]
    public void Construct(Player target)
    {
        _target = target.transform;
    }

    private void Update()
    {
        if (_target)
        {
            MoveToTarget(_target);
        }
    }

    private void MoveToTarget(Transform target)
    {
        if (Vector2.Distance(transform.position, target.position) <= _maxOffset)
        {
            Vector2 newPosition = Vector2.SmoothDamp(transform.position, target.position, ref _velocity, _timeSmooth, _speedFollowing);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
        else
        {
            transform.position = (Vector3)target.position + Vector3.forward * transform.position.z;
        }
    }
}
