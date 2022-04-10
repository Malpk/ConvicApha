using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    private Health _health;
    private Rigidbody2D _rb2d;
    private AudioSource _audioSrc;
    private Camera _mainCamera;

    [SerializeField]
    private float _multiplierSpeed = 5;
    [SerializeField]
    private float _multiplierSpeedRotation = 10f;
    [SerializeField]
    private float _multiplierSpeedCamera = 10f;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _audioSrc = GetComponent<AudioSource>();
        _health = GetComponent<Health>();
        _mainCamera = Camera.main;
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
        _rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * _multiplierSpeed, Input.GetAxis("Vertical") * _multiplierSpeed);
        if (_rb2d.velocity.magnitude >= _multiplierSpeed)
        {
            _rb2d.velocity = _rb2d.velocity.normalized * _multiplierSpeed;
        }
    }

    // Поворот модельки относительно движения
    private void Rotate()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!_audioSrc.isPlaying)
            {
                _audioSrc.Play();
            }

            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, _rb2d.velocity.normalized), _multiplierSpeedRotation * Time.deltaTime);
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
