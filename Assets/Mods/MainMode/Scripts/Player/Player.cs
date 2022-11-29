using UnityEngine;
using Zenject;
using PlayerComponent;
using MainMode;
using MainMode.GameInteface;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Player : MonoBehaviour, IDamage, IResist, IMovement
{
    [Min(1)]
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private bool _playOnStart = true;
    [Header("General Setting")]
    [SerializeField] private Collider2D _colliderBody;
    [SerializeField] private ShootMarkerView _shootMarker;
    [SerializeField] private PlayerState _state;
    [SerializeField] private PlayerBaseBehaviour _behaviour;
    [SerializeField] private PCPlayerController _contraller;
    [SerializeField] private PlayerEffectSet _effects;

    private HUDUI _hud;
    private Rigidbody2D _rigidBody;
    private PlayerMovement _playerMovement = new PlayerMovement();

    private IPlayerComponent[] _component;
    private Transport _seedTransport;
    private Vector3 _startPosition;

    private IItemInteractive _interacive = null;

    public event System.Action DeadAction;
    public event System.Action<int> OnSetupMaxHealth;
    public event System.Action<int> OnUpdateHealth;

    public bool IsPlay { get; private set; } = false;
    public string Name => _behaviour.Name;
    public Vector2 Position => transform.position;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
        _playerMovement.Intializate(this, _rigidBody);
        _component = GetComponents<IPlayerComponent>();
    }
    [Inject]
    public void Construct(HUDUI hud)
    {
        _hud = hud;
        if (_behaviour)
            _behaviour.Intializate(this, hud, _maxHealth);
    }

    public void SetBehaviour(PlayerBaseBehaviour behaviour, int health)
    {
        if (_behaviour != behaviour && _behaviour)
        {
            Destroy(_behaviour.gameObject);
        }
        _maxHealth = health;
        _behaviour = behaviour;
        _behaviour.Intializate(this, _hud, health);
        _behaviour.transform.parent = transform;
        _behaviour.transform.localPosition = Vector3.zero;
        _behaviour.transform.localRotation = Quaternion.Euler(Vector3.zero);;
        enabled = true;
    }


    private void Start()
    {
        _contraller.SetMovement(this);
        if (_playOnStart)
        {
            OnSetupMaxHealth?.Invoke(_maxHealth);
            Play();
        }
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
            transform.position = _startPosition;
            _rigidBody.rotation = 0;
            foreach (var component in _component)
            {
                component.Play();
            }
            _behaviour.gameObject.SetActive(true);
            _behaviour.Play();
            UnBlock();
        }
    }

    public void Stop()
    {
        _behaviour.Stop();
        _behaviour.gameObject.SetActive(false);
    }
    public void Block()
    {
        _contraller.Block();
    }

    public void UnBlock()
    {
        _contraller.UnBlock();
    }
    public void EnterToTransport(Transport transport)
    {
        _seedTransport = transport;
        transport.Enter(this, _rigidBody, _contraller);
    }
    public void ExitToTransport()
    {
        _seedTransport = null;
        transform.parent = null;
    }
    public void SetImpactDamage(bool mode)
    {
        _colliderBody.enabled = mode;
    }
    #endregion
    #region Change Health
    public void Explosion()
    {
        if (IsPlay)
        {
            IsPlay = false;
            Block();
            _behaviour.Dead();
            _rigidBody.velocity = Vector2.zero;
        }
    }
    public void TakeDamage(int damage, DamageInfo damgeInfo)
    {
        if (IsPlay)
        {
            if (_behaviour.TakeDamage(damage, damgeInfo))
            {
                _effects.AddEffectDamage(damgeInfo);
                OnUpdateHealth?.Invoke(_behaviour.Health);
            }
            else if (_behaviour.Health == 0)
            {
                Explosion();
            }
        }
    }
    public void Heal(int value)
    {
        if (_behaviour.Heal(value))
            OnUpdateHealth(_behaviour.Health);
    }
    private void DeadMessange()
    {
        if (DeadAction != null)
            DeadAction();
    }
    #endregion
    #region Add Effects
    public void AddResistAttack(DamageInfo damage, float timeActive)
    {
        _behaviour.AddResist(damage, timeActive);
    }
    #endregion
    #region Interactive and Useble


    public void UseAbillity()
    {
        var ability = _behaviour.UseAbillity();
        if (ability)
        {
            if (ability.IsUseRotation)
                transform.rotation = Quaternion.Euler(Vector3.forward * (_shootMarker.Angel - 90));
            ability.Use();
        }
    }

    public void InteractiveWhithObject()
    {
        if (_interacive != null)
        {
            _interacive.Interactive(this);
        }
    }
    #endregion
    public void MoveToPosition(Vector2 position)
    {
        if (!_seedTransport)
        {
            _rigidBody.MovePosition(position);
            return;
        }
        _seedTransport.transform.position = position;
    }
    public void Move(Vector2 input)
    {
        if (input == Vector2.zero) 
            return;
        _rigidBody.AddForce(input * _state.SpeedMovement * _effects.MoveEffect, ForceMode2D.Force);
        _rigidBody.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, input), _state.SpeedMovement * _effects.MoveEffect * Time.deltaTime));
    }
    private void HideScreen(EffectType effect)
    {
        //if (effect == EffectType.Freez)
        //    _animator.SetBool("Freez", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IItemInteractive interactive))
        {
            _interacive = interactive;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IItemInteractive interactive))
        {
            _interacive = null;
        }
    }
}

