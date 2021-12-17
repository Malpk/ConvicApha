using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerSpace;


[RequireComponent(typeof(Animator),typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speedMovement = 7f;
    [SerializeField] private float _speedRotation;
    [SerializeField] private PCController _pcCpntroller;

    private IGameController _controller;

    public GameObject DieMenu;

    public bool CanMove = true;

    private bool _isDead;
    private Animator _animator;
    private IMovement _movement;
    private IRotate _rotate;
    private PlayerRotation _playerRotate;


    private void Awake()
    {
        _controller = _pcCpntroller;
        _isDead = false;
        _animator = GetComponent<Animator>();
        var rigidbody = GetComponent<Rigidbody2D>();
        _movement = new PhysicMovement(rigidbody, _speedMovement);
        _rotate = new PlayerRotation(_speedRotation);
    }

    void Start()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
       
    }
    void FixedUpdate()
    {
        if (!_isDead)
        {
            var direction = _controller.MovementInput;
            transform.rotation = Quaternion.Inverse( _rotate.Rotate(_controller.MovementInput));
           _movement.Move(direction);
        }

    }

    public void Term()
    {
        if (_isDead)
            return;
        _isDead = true;
        _animator.SetTrigger("dead");
    }
    public void Dead()
    {
        Debug.Log("ss");
        DieMenu.SetActive(true);
        Time.timeScale = 0;
    }

}
