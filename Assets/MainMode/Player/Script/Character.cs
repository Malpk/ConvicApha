using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;
using MainMode.GameInteface;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour, IMoveEffect, IDamage, ISender
{
    [Header("Movement Setting")]
    [Min(1)]
    [SerializeField] protected float speedMovement = 1f;
    [Min(1)]
    [SerializeField] protected float speedRotaton = 1f;
    [SerializeField] protected PlayerHealth health;
    [Header("Respawn Setting")]
    [SerializeField] protected bool isAutoRespawnMode = true;
    [Min(0)]
    [SerializeField] protected float respawnTime = 1f;

    private Vector3 _startPosition;

    protected bool isDead = false;
    protected Animator animator;
    protected Rigidbody2D rigidBody;
    protected PlayerMovement _movement;
    protected IPlayerComponent[] _component;

    protected Coroutine respawn = null;

    public int Health => health.Health;
    public abstract bool IsUseEffect { get; }
    public abstract bool IsDead { get; }
    public Vector2 Position => transform.position;
    public Quaternion Rotation => transform.rotation;

    public TypeDisplay TypeDisplay => TypeDisplay.HealthUI;

    protected virtual void Awake()
    {
        _startPosition = transform.position;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        _movement = new PlayerMovement(this, rigidBody);
        _component = GetComponents<IPlayerComponent>();
    }
    protected virtual void Start()
    {
        health.Start();
    }
    public bool AddReceiver(Receiver receiver)
    {
        if (receiver is HealthUI display)
        {
            return health.SetReceiver(display);
        }
        return false;
    }
    protected abstract void Move(Vector2 direction);
    public abstract void Dead();
    public abstract void TakeDamage(int damage,  DamageInfo type);
    public abstract void StopMove(float timeStop, EffectType effect = EffectType.None);
    public abstract void ChangeSpeed(float duration,EffectType effect, float value = 1);

    protected IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(respawnTime);
        Respawn();
        respawn = null;
    }

    public virtual void Respawn()
    {
        isDead = false;
        transform.position = _startPosition;
        animator.SetBool("Dead", false);
        foreach (var component in _component)
        {
            component.Play();
        }
        rigidBody.rotation = 0;
        health.Heal(health.MaxHealth);
    }


}
