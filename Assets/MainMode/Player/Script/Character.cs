using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;
using MainMode.GameInteface;
using PlayerComponent;
using MainMode.Effects;
using MainMode.Items;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour, IAddEffects, IDamage, ISender
{
    [Header("Movement Setting")]
    [Min(1)]
    [SerializeField] protected float speedMovement = 1f;
    [Min(1)]
    [SerializeField] protected float speedRotation = 1f;
    [SerializeField] protected PlayerHealth health;
    [Header("Respawn Setting")]
    [SerializeField] protected bool isAutoRespawnMode = true;
    [Min(0)]
    [SerializeField] protected float respawnTime = 1f;
    [SerializeField] protected PlayerMovement _baseMovement;

    protected bool isDead = false;
    protected Animator animator;
    protected Rigidbody2D rigidBody;
    protected IPlayerComponent[] _component;
    protected IMovement movement;

    protected Coroutine respawn = null;

    private Vector3 _startPosition;
    private Coroutine _changeMovement = null;
    private Dictionary<MovementEffect, int> _movementEffects = new Dictionary<MovementEffect, int>();

    public int Health => health.Health;
    public bool IsUseEffect { get; protected set; } = true;

    public abstract bool IsDead { get; }
    public Vector2 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
    public TypeDisplay TypeDisplay => TypeDisplay.HealthUI;

    protected virtual void Awake()
    {
        _startPosition = transform.position;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        _component = GetComponents<IPlayerComponent>();
        _baseMovement.Intializate(this, rigidBody);
        movement = _baseMovement;
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
    #region Add Effects
    public void AddEffects(MovementEffect effect,float timeActive, EffectType type = EffectType.None)
    {
        if (!_movementEffects.ContainsKey(effect))
            _movementEffects.Add(effect, 1);
        else
            _movementEffects[effect]++;
        StartCoroutine(DeleteEffect(timeActive, _movementEffects, effect, type));
    }
    private void SetAnimationEffect(EffectType effect, bool mode)
    {
        switch (effect)
        {
            case EffectType.Freez:
                animator.SetBool("Freez", mode);
                return;
        }
    }
    public bool AddEffects(ITransport transport,float timeActive)
    {
        if (_changeMovement == null)
        {
            movement = transport;
            _changeMovement = StartCoroutine(ResetMovement(transport, timeActive));
            return true;
        }
        return false;
    }
    protected float GetMovementEffect()
    {
        if (_movementEffects.Count == 0 && IsUseEffect)
            return 1f;
        var amount = 1f;
        foreach (var effect in _movementEffects)
        {
            amount *= effect.Key.Effect;
            if (amount == 0)
                return 0;
        }
        return amount;
    }

    private IEnumerator DeleteEffect<T>(float timeActive, Dictionary<T,int> stack, T effect, EffectType type = EffectType.None)
    {
        SetAnimationEffect(type, true);
        var progress = 0f;
        while (!isDead && progress <= 1f)
        {
            progress += Time.deltaTime / timeActive;
            yield return null;
        }
        stack[effect]--;
        if (stack[effect] == 0)
        {
            if (type != EffectType.None)
                SetAnimationEffect(type, false);
            stack.Remove(effect);
        }
    }
    private IEnumerator ResetMovement(ITransport transport,float timeActive)
    {
        var progress = 0f;
        while (!isDead && progress <= 1f)
        {
            progress += Time.deltaTime / timeActive;
            yield return null;
        }
        movement = _baseMovement;
        _changeMovement = null;
        transport.Exit();
        rigidBody.isKinematic = false;
    }

    #endregion
}
