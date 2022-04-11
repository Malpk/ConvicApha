using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMove : UnitMove
{
    private Rigidbody2D _rb2d;
    private AudioSource _audioSrc;
    private Camera _mainCamera;
    private Vector2 _currentDirection;

    [SerializeField]
    protected float _multiplierSpeedCamera = 10f;

    protected override void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _audioSrc = GetComponent<AudioSource>();
        _mainCamera = Camera.main;
        base.Awake();
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        Rotate();
    }

    private void LateUpdate()
    {
       PullTheCamera();
    }

    // Движение игрока
    private void Move()
    {
        _currentDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rb2d.velocity = _currentDirection * _currentSpeed;
        if (_rb2d.velocity.magnitude >= _currentSpeed)
        {
            _rb2d.velocity = _rb2d.velocity.normalized * _currentSpeed;
        }

    }

    // Поворот модельки относительно движения
    private void Rotate()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //if (!_audioSrc.isPlaying)
            //{
            //    _audioSrc.Play();
            //}

            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, _currentDirection), _multiplierSpeedRotation * Time.deltaTime);
            _rb2d.MoveRotation(rotation);
        }
    }

    // Следование камеры за игроком
    private void PullTheCamera()
    {
        Vector2 newPosition = Vector2.Lerp(_mainCamera.transform.position, transform.position, _multiplierSpeedCamera * Time.deltaTime);
        _mainCamera.transform.position = new Vector3(newPosition.x, newPosition.y, _mainCamera.transform.position.z);
    }
}
