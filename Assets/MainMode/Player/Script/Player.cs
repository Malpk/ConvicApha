using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IMoveEffect, IDamage
{
    [Header("Movement Setting")]
    [Min(1)]
    [SerializeField] private float _speedMovement = 1f;
    [Min(1)]
    [SerializeField] private float _speedRotaton = 1f;
    [SerializeField] private PlayerHealth _health;
#if UNITY_EDITOR
    [Header("Debug")]
    [Min(0)]
    [SerializeField] private float _respawnTime = 1f;

    private Vector3 _startPosition;
    private Coroutine _respawn = null;
#endif
    private bool _isDead = false;

    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private PlayerMovement _movement;
    
    private Coroutine _stopMoveCorotine;
    private Coroutine _utpdateMoveCorotine;

    public int PlayerHealth { get; private set; }
    public Vector2 Position => transform.position;
    public Quaternion Rotation => transform.rotation;


    public delegate void DeadEvent();
    public event DeadEvent DeadAction;

    private void Awake()
    {
#if UNITY_EDITOR
        _startPosition = transform.position;
#endif
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = new PlayerMovement(this, _rigidBody);
    }
    private void Start()
    {
        _health.Start();
    }
    private void Update()
    {
        if (_isDead)
            return;
        var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _movement.Move(direction * _speedMovement);
        if (direction != Vector2.zero)
            _movement.Rotate(direction,_speedRotaton);
    }

    public void Dead()
    {
        _isDead = true;
        _animator.SetTrigger("Dead");
        _health.EventOnDead.Invoke();
        _rigidBody.velocity = Vector2.zero;
#if UNITY_EDITOR
        if(_respawn == null)
            _respawn = StartCoroutine(ReSpawn());
#endif
    }
    public void TakeDamage(int damage)
    {
        _health.SetDamage(damage);
        if (_health.Health <= 0)
        {
            Dead();
        }
        else
        {
            _health.EventOnTakeDamage.Invoke();
        }
    }
    public void StopMove(float timeStop)
    {
        if (_stopMoveCorotine == null)
            _stopMoveCorotine = StartCoroutine(StopMovement(timeStop));
    }
    public void ChangeSpeed(float duration, float value = 1)
    {
        if (_utpdateMoveCorotine == null)
            _utpdateMoveCorotine = StartCoroutine(UpdateSpeed(duration, value));
    }
    private IEnumerator StopMovement(float duratuin)
    {
        var temp = _speedMovement;
        _speedMovement = 0f;
        _animator.SetBool("Freez",true);
        yield return new WaitForSeconds(duratuin);
        if (_utpdateMoveCorotine == null)
            _speedMovement = temp;
        _animator.SetBool("Freez", false);
        _stopMoveCorotine = null;
    }
    private IEnumerator UpdateSpeed(float duration, float value)
    {
        var temp = _speedMovement;
        _speedMovement *= value;
        yield return new WaitForSeconds(duration);
        if (_stopMoveCorotine == null)
            _speedMovement = temp;
        _utpdateMoveCorotine = null;
    }
#if UNITY_EDITOR
    private IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(_respawnTime);
        transform.position = _startPosition;
        _rigidBody.rotation = 0;
        _health.Heal(_health.MaxHealth);
        _animator.SetTrigger("Live");
        _isDead = false;
        _respawn = null;
    }
#endif
}
