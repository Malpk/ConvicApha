using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour, IMoveEffect, IDamage
{
    [Header("Movement Setting")]
    [Min(1)]
    [SerializeField] protected float speedMovement = 1f;
    [Min(1)]
    [SerializeField] protected float speedRotaton = 1f;
    [SerializeField] protected PlayerHealth health;
    [Header("Debug")]
    [Min(0)]
    [SerializeField] protected float respawnTime = 1f;

    private Vector3 _startPosition;

    protected bool isDead = false;
    protected Animator animator;
    protected Rigidbody2D rigidBody;
    protected PlayerMovement _movement;
    protected IPlayerComponent[] _component;

    protected Coroutine respawn = null;

    public abstract bool IsUseEffect { get; }
    public abstract bool IsDead { get; }
    public Vector2 Position => transform.position;
    public Quaternion Rotation => transform.rotation;

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
    protected abstract void Move(Vector2 direction);
    public abstract void Dead();
    public abstract void TakeDamage(int damage,  AttackInfo type);
    public abstract void StopMove(float timeStop, EffectType effect = EffectType.None);
    public abstract void ChangeSpeed(float duration,EffectType effect, float value = 1);

    protected IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(respawnTime);
        transform.position = _startPosition;
        isDead = false;
        animator.SetBool("Dead",false);
        ResetCharacter();
        respawn = null;
        foreach (var component in _component)
        {
            component.Play();
        }
    }

    protected virtual void ResetCharacter()
    {
        rigidBody.rotation = 0;
        health.Heal(health.MaxHealth);
    }
}
