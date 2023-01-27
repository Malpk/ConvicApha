using UnityEngine;

public class MovingWeapon : MonoBehaviour
{
    [Min(1f)]
    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private float _moveDistance;
    [SerializeField] private Vector2 _directon;
    [Header("Reference")]
    [SerializeField] private Rigidbody2D _rigidBody;

    private float _progress = 0f;
    private Vector2 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        _progress = Mathf.Clamp01(_progress + Time.fixedDeltaTime / _moveDuration);
        _rigidBody.MovePosition(_startPosition + _directon * _moveDistance * _progress);
        if (_progress == 1)
        {
            _directon = -_directon;
            _progress = 0f;
            _startPosition = _rigidBody.position;
        }
    }

    public void Play()
    {
        enabled = true;
    }
    public void Stop()
    {
        enabled = false;
    }
}
