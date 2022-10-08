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
    [SerializeField] private bool _playOnStart = true;
    [Header("Movement Setting")]
    [SerializeField] protected PlayerState state;
    [SerializeField] protected PlayerHealth health;
    [Header("Respawn Setting")]
    [SerializeField] protected bool isAutoRespawnMode = true;
    [Min(0)]
    [SerializeField] protected float respawnTime = 1f;
    [SerializeField] protected PlayerMovement _baseMovement;

    protected Animator animator;
    protected Rigidbody2D rigidBody;
    protected IPlayerComponent[] _component;
    protected IMovement movement;

    protected Coroutine respawn = null;

    private Vector3 _startPosition;
    private Coroutine _changeMovement = null;
    private Dictionary<MovementEffect, int> _movementEffects = new Dictionary<MovementEffect, int>();

    public event System.Action RespawnAction;

    protected event System.Action PlayAction;
    protected event System.Action StopAction;

    public int Health => health.Health;
    public bool IsUseEffect { get; protected set; } = true;

    public bool IsPlay { get; private set; } = false;
    public Vector2 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
    public TypeDisplay TypeDisplay => TypeDisplay.HealthUI;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
        _baseMovement.Intializate(this, rigidBody);
        _component = GetComponents<IPlayerComponent>();
        movement = _baseMovement;
    }
    private void Start()
    {
        if (_playOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        if (!IsPlay)
        {
            IsPlay = true;
            transform.position = _startPosition;
            animator.SetBool("Dead", false);
            rigidBody.rotation = 0;
            foreach (var component in _component)
            {
                component.Play();
            }
            if (health.IsLoadDisplay)
            {
                health.Intializate();
                health.Heal(health.MaxHealth);
            }
            if (PlayAction != null)
                PlayAction();
        }
    }

    public void Stop()
    {
        if (IsPlay)
        {
            IsPlay = false;
            if (StopAction != null)
                StopAction();
        }
    }

    public bool AddReceiver(Receiver receiver)
    {
        if (receiver is HealthUI display)
        {
            health.SetReceiver(display);
            health.Intializate();
            return true;
        }
        return false;
    }
    protected abstract void Move(Vector2 direction);
    public abstract void Explosion();
    public abstract void TakeDamage(int damage,  DamageInfo type);

    protected IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(respawnTime);
        Play();
        respawn = null;
    }
    #region Add Effects
    public virtual void AddEffects(MovementEffect effect,float timeActive)
    {
        if (!_movementEffects.ContainsKey(effect))
            _movementEffects.Add(effect, 1);
        else
            _movementEffects[effect]++;
        StartCoroutine(DeleteEffect(timeActive, _movementEffects, effect, effect.Effect));
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
            amount *= effect.Key.Value;
            if (amount == 0)
                return 0;
        }
        return amount;
    }

    private IEnumerator DeleteEffect<T>(float timeActive, Dictionary<T,int> stack, T effect, EffectType type = EffectType.None)
    {
        SetAnimationEffect(type, true);
        var progress = 0f;
        while (IsPlay && progress <= 1f)
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
        while (IsPlay && progress <= 1f)
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
