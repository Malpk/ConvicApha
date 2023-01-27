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
    [SerializeField] private PlayerInvulnerability _playerInvulnerability;
    [SerializeField] private PlayerBaseAbillitySet _ability;
    [Header("Reference")]
    [SerializeField] private Collider2D _collider;

    private IPlayerComponent[] _components;

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
        _ability = set;
        if (set is AbillityActiveSet abillity)
            _contraller.SetAbillity(abillity);
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
            _ability.Play();
            UnBlock();
        }
    }

    public void Stop()
    {
        IsPlay = false;
        _behaviour.Stop();
        _behaviour.gameObject.SetActive(false);
        foreach (var component in _components)
        {
            component.Stop();
        }
        _ability.Stop();
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
        if(!_playerInvulnerability.IsInvulnerability)
            _behaviour.Explosion();
    }
    public bool Heal(int heal)
    {
        if (_behaviour.Heal(heal))
        {
            _effects.AddEffect(EffectType.Heal, 1f);
            return true;
        }
        else
        {
            return false;
        }

    }
    public void MoveToPosition(Vector2 position)
    {
        _movement.MoveToPosition(position);
    }
    public void TakeDamage(int damage, DamageInfo damageInfo)
    {
        if (!_playerInvulnerability.IsInvulnerability)
        {
            if (_behaviour.TakeDamage(damage, damageInfo))
            {
                if (_behaviour.Health > 0)
                {
                    _effects.AddEffectDamage(damageInfo);
                    if (damage > 0)
                    {
                        _behaviour.Invulnerability(true);
                        _playerInvulnerability.ActivateForWhite(() => _behaviour.Invulnerability(false));
                    }
                }
                else
                {
                    Block();
                    Explosion();
                }
            }
        }
    }

    public void AddEffect(MovementEffect effects, float timeActive)
    {
        if (!_playerInvulnerability.IsInvulnerability)
            _effects.AddEffect(effects, timeActive);
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
        if(mode)
            _playerInvulnerability.Activate();
        else
            _playerInvulnerability.Deactivate();
        _behaviour.Invulnerability(mode);
    }
    public void SetModeCollider(bool mode)
    {
        _collider.enabled = mode;
    }

    public void ResetState()
    {
        foreach (var component in _components)
        {
            component.ResetState();
        }
        _ability.ResetState();
    }
}

