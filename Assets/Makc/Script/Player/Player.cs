using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using Zenject;
using Underworld;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D),typeof(AudioSource))]
public class Player : MonoBehaviour
{
    [Header("Game Setting")]
    [SerializeField] private float _speedMovement = 7f;
    [SerializeField] private float _speedRotation;
    [Header("Perfab Setting")]
    [SerializeField] private PCController _pcCpntroller;
    [SerializeField] private AndroidControl _android;
    [SerializeField] private CameraFolowing _camera;
    [SerializeField] private AudioClip _deadSound;

    private bool _immortalityMode = false;
    private GameEvent _gameEvent;
    private Animator _animator;
    private AudioSource _soundSource;
    private LvlSate _curretState = LvlSate.Play;
    private IMovement _movement;
    private IRotate _rotate;
    private IGameController _controller;
    private PlayerSate _playerState = PlayerSate.Live;

    public delegate void Dead();
    public event Dead DeadAction;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        _soundSource = GetComponent<AudioSource>();
        _soundSource.playOnAwake = false;
        _soundSource.loop = false;
        _soundSource.clip = _deadSound;
        _controller = _pcCpntroller;
        _animator = GetComponent<Animator>();
        var rigidbody = GetComponent<Rigidbody2D>();
        _movement = new PhysicMovement(rigidbody, _speedMovement);
        _rotate = new PlayerRotate(_speedRotation);
    }
    [Inject]
    private void Constructor(GameEvent gameEvent)
    {
        _gameEvent = gameEvent;
    }
    private void OnEnable()
    {
        if (_gameEvent != null)
        {
            _gameEvent.StartAction += SetGameState;
            _gameEvent.WinAction += () => _immortalityMode = true;
            _gameEvent.StopAction += SetPause;
        }
    }
    private void OnDisable()
    {
        if (_gameEvent != null)
        {
            _gameEvent.StartAction -= SetGameState;
            _gameEvent.WinAction -= () => _immortalityMode = true;
            _gameEvent.StopAction -= SetPause;
        }
    }
    private void Start()
    {
        if (_gameEvent != null)
            _curretState = _gameEvent.State;
    }
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
        if (hit)
        {
            if (hit.transform.TryGetComponent<ITileType>(out ITileType tile))
                DefineTile(tile);
        }
    }
    private void FixedUpdate()
    {
        if (_curretState == LvlSate.Play && _playerState != PlayerSate.Dead)
        {
            var direction = _controller.MovementInput;
            transform.rotation *= Quaternion.Inverse(_rotate.Rotate(_controller.MovementInput));
            _movement.Move(direction);
        }
        else
        {
            _movement.Move(Vector2.zero);
        }
    }
    private void DefineTile(ITileType tile)
    {
        switch (tile.tileType)
        {
            case TypeTile.TernTile:
                Incineration();
                return;
            default:
                break;
        }
    }
    private void SetGameState()
    {
        _curretState = LvlSate.Play;
        var x = _camera.CameraPosition.x;
        var y = _camera.CameraPosition.y;
        transform.position = new Vector3(x,y,transform.position.z);
    }
    public void Incineration()
    {
        _soundSource.Play();
        if (_playerState == PlayerSate.Dead || _immortalityMode)
            return;
        _animator.SetTrigger("dead");
        _playerState = PlayerSate.Dead;
    }
    private void DeadPlayer()
    {
        Time.timeScale = 0f;
        _soundSource.Stop();
        if (DeadAction != null)
            DeadAction();
    }
    private void SetPause()
    {
        _curretState = LvlSate.Pause;
    }
}
