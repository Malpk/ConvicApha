using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;
using GameMode;
using Zenject;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speedMovement = 7f;
    [SerializeField] private float _speedRotation;
    [SerializeField] private PCController _pcCpntroller;

    private IGameController _controller;

    private Animator _animator;
    private CameraAnimation _cameraAnimation;
    private GameState _curretState;
    private IMovement _movement;
    private IRotate _rotate;

    public delegate void Dead(GameState state);
    public event Dead DeadAction;

    private void Awake()
    {
        _controller = _pcCpntroller;
        _animator = GetComponent<Animator>();
        var rigidbody = GetComponent<Rigidbody2D>();
        _movement = new PhysicMovement(rigidbody, _speedMovement);
        _rotate = new PlayerRotate(_speedRotation);
    }
    [Inject]
    private void Constructor(CameraAnimation camera)
    {
        _cameraAnimation = camera;
    }
    private void OnEnable()
    {
        _cameraAnimation.CompliteAction += SetGameState;
    }
    private void OnDisable()
    {
        _cameraAnimation.CompliteAction -= SetGameState;
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
        if (_curretState == GameState.Play)
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
        _curretState = GameState.Play;
    }
    public void Incineration()
    {
        if (_curretState == GameState.Dead)
            return;
        _animator.SetTrigger("dead");
        _curretState = GameState.Dead;
    }
    private void DeadPlayer()
    {
        Time.timeScale = 0f;
        if (DeadAction != null)
            DeadAction(GameState.Dead);
    }
}
