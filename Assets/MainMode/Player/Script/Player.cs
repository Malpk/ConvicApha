using UnityEngine;
using Zenject;
using PlayerComponent;
using MainMode;
using MainMode.Items;
using MainMode.GameInteface;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Player : MonoBehaviour, IAddEffects, IDamage, IResist,IBlock
{
    [SerializeField] private bool _playOnStart = true;
    [Header("General Setting")]
    [SerializeField] private Controller _controller;
    [SerializeField] private Collider2D _colliderBody;
    [SerializeField] private ShootMarkerView _shootMarker;
    [SerializeField] private PlayerState _state;
    [SerializeField] private PlayerBaseBehaviour _behaviour;

    private HUDInteface _hud;
    private Rigidbody2D _rigidBody;
    private PlayerMovement _playerMovement = new PlayerMovement();
    private IPlayerComponent[] _component;
    private ITransport _transport;

    private Vector3 _startPosition;
    private PlayerInventory _inventory = new PlayerInventory();

    private IItemInteractive _interacive = null;

    public event System.Action DeadAction;

    public bool IsPlay { get; private set; } = false;
    public Vector2 Position => transform.position;
    public float MovementEffect => _behaviour.MoveEffect;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
        _playerMovement.Intializate(this, _rigidBody);
        _component = GetComponents<IPlayerComponent>();
    }
    [Inject]
    public void Construct(HUDInteface hud, Controller controller, DeadMenu deadmenu)
    {
        _hud = hud;
        _controller = controller;
        if (_behaviour)
            _behaviour.Intializate(this, hud);
    }


    public void SetBehaviour(PlayerBaseBehaviour behaviour)
    {
        if (_behaviour)
        {
            Destroy(_behaviour.gameObject);
        }
        _behaviour = behaviour;
        _behaviour.Intializate(this, _hud);
        _behaviour.transform.parent = transform;
        _behaviour.transform.localPosition = Vector3.zero;
        _behaviour.transform.localRotation = Quaternion.Euler(Vector3.zero);;
        enabled = true;
    }
    #region Intilizate
    private void OnEnable()
    {
        BindController(_controller);
        _behaviour.DeadAction += DeadMessange;
    }
    private void OnDisable()
    {
        UnBindController(_controller);
        _behaviour.DeadAction -= DeadMessange;
    }
    private void BindController(Controller controller)
    {
        controller.InteractiveAction += InteractiveWhithObject;
        controller.UseItemAction += UseItem;
        controller.UseArtifactAction += UseArtifact;
        controller.MovementAction += Move;
        controller.UseAbillityAction += UseAbillity;
    }
    private void UnBindController(Controller controller)
    {
        controller.InteractiveAction -= InteractiveWhithObject;
        controller.UseItemAction -= UseItem;
        controller.UseArtifactAction -= UseArtifact;
        controller.MovementAction -= Move;
        controller.UseAbillityAction -= UseAbillity;
    }
    #endregion
    private void Start()
    {
        if (_playOnStart)
        {
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
            _behaviour.Play();
            _controller.UnBlock();
        }
    }

    public void Stop()
    {
        if (IsPlay)
        {
            IsPlay = false;
            _behaviour.Stop();
            _controller.Block();
        }
    }
    public void Block()
    {
        _controller.MovementAction -= Move;
    }

    public void UnBlock()
    {
        _controller.MovementAction += Move;
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
            Stop();
            _behaviour.Dead();
            _rigidBody.velocity = Vector2.zero;
        }
    }
    public void TakeDamage(int damage, DamageInfo damgeInfo)
    {
        if (_behaviour.TakeDamage(damage, damgeInfo))
        {
            _hud.SetHealth(_behaviour.Health);
            if (_behaviour.Health == 0)
                Explosion();
        }
    }
    public void Heal(int value)
    {
        _behaviour.Heal(value);
    }
    private void DeadMessange()
    {
        if (DeadAction != null)
            DeadAction();
    }
    #endregion
    #region Add Effects
    public void AddEffects(MovementEffect effect,float timeActive)
    {
        _behaviour.AddEffects(effect, timeActive);
    }
    public void AddResistAttack(DamageInfo damage, float timeActive)
    {
        _behaviour.AddResist(damage, timeActive);
    }

    public bool SetTransport(ITransport transport)
    {
        if (_transport == null)
        {
            _transport = transport;
            _transport.Enter(this ,_rigidBody ,_controller);
            return true;
        }
        return false;
    }
    #endregion
    #region Interactive and Useble
    public void PickItem(Item itemUse)
    {
        if (itemUse is ConsumablesItem consumablesItem)
        {
            itemUse.Pick(this);
            _hud.DisplayConsumableItem(consumablesItem);
            _inventory.AddConsumablesItem(consumablesItem);
        }
        else
        {
            itemUse.Pick(this);
            _hud.DisplayArtifact(itemUse);
            _inventory.AddArtifact(itemUse);
        }
       
    }
    private void UseItem()
    {
        if (_inventory.TryGetConsumableItem(out ConsumablesItem item))
        {
            if(!item.Use())
                _hud.DisplayConsumableItem(null);
        }
    }
    private void UseArtifact()
    {
        if (_inventory.TryGetArtifact(out Item artifact))
        {
            if (artifact.UseType == ItemUseType.Shoot)
                transform.rotation = Quaternion.Euler(Vector3.forward * (_shootMarker.Angel - 90));
            if (!artifact.Use())
                _hud.DisplayArtifact(null);
        }
    }
    private void UseAbillity()
    {
        _behaviour.UseAbillity();
    }

    private void InteractiveWhithObject()
    {
        if (_interacive != null)
        {
            _interacive.Interactive(this);
        }
    }
    #endregion
    public void MoveToPosition(Vector2 position)
    {
        _rigidBody.MovePosition(position);
    }
    private void Move(Vector2 direction)
    {
        _playerMovement.Move(direction * _state.SpeedMovement * _behaviour.MoveEffect);
        if (direction != Vector2.zero)
            _playerMovement.Rotate(direction, _state.SpeedMovement * _behaviour.MoveEffect);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Item itemUse))
        {
            PickItem(itemUse);
        }
        else if (collision.TryGetComponent(out IItemInteractive interactive))
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

