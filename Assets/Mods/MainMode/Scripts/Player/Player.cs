using UnityEngine;
using PlayerComponent;

public sealed class Player : MonoBehaviour, IDamage, IAddEffects, IResist
{
    [SerializeField] private bool _playOnStart = true;
    [Header("General Setting")]
    [SerializeField] private PlayerBehaviour _behaviour;
    [SerializeField] private PCPlayerController _contraller;
    [SerializeField] private PlayerEffectSet _effects;
    [SerializeField] private PlayerMovementSet _movement;

    private IPlayerComponent[] _components;

    private AbilityPassiveSet _pasiveAbillity;

    public bool IsPlay { get; private set; } = false;
    public Vector2 Position => transform.position;

    private void Awake()
    {
        _components = GetComponents<IPlayerComponent>();
    }

    public void SetBehaviour(PlayerBehaviour behaviour)
    {
        if (_behaviour != behaviour && _behaviour)
        {
            _behaviour.OnDead.RemoveListener(Explosion);
            Destroy(_behaviour.gameObject);
        }
        _behaviour = behaviour;
        _behaviour.transform.parent = transform;
        _behaviour.transform.localPosition = Vector3.zero;
        _behaviour.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _behaviour.OnDead.AddListener(Explosion);
        enabled = true;
    }
    public void SetAbillity(PlayerBaseAbillitySet set)
    {
        set.SetUser(this);
        if (set is AbillityActiveSet active)
        {
            _contraller.SetAbillity(active);
            _pasiveAbillity = null;
        }
        else if (set is AbilityPassiveSet passive)
        {
            _pasiveAbillity = passive;
            _contraller.SetAbillity(null);
        }
        else
        {
            _pasiveAbillity = null;
            _contraller.SetAbillity(null);
        }
    }
    private void Start()
    {
        if (_playOnStart)
            Play();
    }
    private void Update()
    {
        _behaviour.UpdateBehaviour();
    }
    #region Controller
    public void Play()
    {
        if (!IsPlay)
        {
            IsPlay = true;
            foreach (var component in _components)
            {
                component.Play();
            }
            _behaviour.gameObject.SetActive(true);
            _behaviour.Play();
            UnBlock();
            if(_pasiveAbillity)
                _pasiveAbillity.Activate();
        }
    }

    public void Stop()
    {
        _behaviour.Stop();
        _behaviour.gameObject.SetActive(false);
        if (_pasiveAbillity)
            _pasiveAbillity.Deactivate();
    }
    public void Block()
    {
        _contraller.Block();
    }

    public void UnBlock()
    {
        _contraller.UnBlock();
    }
    #endregion

    public void Explosion()
    {
        _behaviour.Explosion();
    }
    public void Heal(int heal)
    {
        _behaviour.Heal(heal);
    }
    public void MoveToPosition(Vector2 position)
    {
        _movement.MoveToPosition(position);
    }
    public void TakeDamage(int damage, DamageInfo damageInfo)
    {
        if (_behaviour.TakeDamage(damage, damageInfo))
        {
            if (_behaviour.Health > 0)
                _effects.AddEffectDamage(damageInfo);
            else
                Block();
        }
    }

    public void AddEffects(MovementEffect effects, float timeActive)
    {
        _effects.AddEffects(effects, timeActive);
    }

    public void AddResistAttack(DamageInfo damage, float timeActive)
    {
        _behaviour.AddResistAttack(damage, timeActive);
    }
    public void ExitToTransport()
    {
        _movement.ExitToTransport();
    }
    public void EnterToTransport(Transport transport)
    {
        _movement.EnterToTransport(transport);
    }

    public void SetState(PlayerState state, bool mode)
    {
        _behaviour.SetAnimation(state, mode);
    }
    public void Invulnerability(bool mode)
    {
        _behaviour.Invulnerability(mode);
    }
}

