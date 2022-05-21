using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class PlayerMove : MonoBehaviour, IMoveEffect
{
    [SerializeField]
    private float _speedMovement = 5f;
    [SerializeField]
    private float _multiplierSpeedRotation = 10f;
    [SerializeField]
    private float _multiplierSpeedCamera = 10f;


    private float _currentSpeed;

    private Rigidbody2D _rb2d;
    private Camera _mainCamera;
    private Vector2 _currentDirection;

    private Coroutine _stopMoveCorotine;
    private Coroutine _utpdateMoveCorotine;

    protected void Awake()
    {
        _currentSpeed = _speedMovement;
        _rb2d = GetComponent<Rigidbody2D>();
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
       FollowingTheCamera();
    }
    private void Move()
    {
        _currentDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rb2d.velocity = _currentDirection * _currentSpeed;
        if (_rb2d.velocity.magnitude >= _currentSpeed)
        {
            _rb2d.velocity = _rb2d.velocity.normalized * _currentSpeed;
        }

    }
    private void Rotate()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, _currentDirection), _multiplierSpeedRotation * Time.deltaTime);
            _rb2d.MoveRotation(rotation);
        }
    }

    private void FollowingTheCamera()
    {
        Vector2 newPosition = Vector2.Lerp(_mainCamera.transform.position, transform.position, _multiplierSpeedCamera * Time.deltaTime);
        _mainCamera.transform.position = new Vector3(newPosition.x, newPosition.y, _mainCamera.transform.position.z);
    }

    public void StopMove(float duration)
    {
        if(_stopMoveCorotine == null)
            _stopMoveCorotine = StartCoroutine(StopMovement(duration));
    }

    public void ChangeSpeed(float duration,float value = 1)
    {
        if(_utpdateMoveCorotine == null)
            _utpdateMoveCorotine = StartCoroutine(UpdateSpeed(duration,value));
    }

    private IEnumerator StopMovement(float duratuin)
    {
        var temp = _currentSpeed;
        _currentSpeed = 0f;
        yield return new WaitForSeconds(duratuin);
        _currentSpeed = temp;
        _stopMoveCorotine = null;
    }
    private IEnumerator UpdateSpeed(float duration, float value)
    {
        var temp = _currentSpeed;
        _currentSpeed *= value;
        yield return new WaitForSeconds(duration);
        _currentSpeed = temp;
        _utpdateMoveCorotine = null;
    }
}
