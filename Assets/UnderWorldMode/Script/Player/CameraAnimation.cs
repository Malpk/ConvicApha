using System.Collections;
using UnityEngine;
using Zenject;
using Underworld;

[RequireComponent(typeof(Animator))]
public class CameraAnimation : MonoBehaviour
{
    [Header("Perfab Setting")]
    [SerializeField] private CameraFolowing _cameraFolowing;

    [Inject] private GameEvent _gameEvent;

    private Animator _animator;

    public Vector3 cameraPosition => transform.position;

    public delegate void Complite();
    public Complite CompliteAction;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        _gameEvent.StartAction += Deactive;
    }
    private void OnDisable()
    {
        _gameEvent.StartAction -= Deactive;
    }
    private void Start()
    {
        var value = _gameEvent.TypeEvent ==  TypeGameEvent.MainMenu;
        Move(value);
    }
    private void Deactive()
    {
        if (CompliteAction != null)
            CompliteAction();
        Move(false);
        enabled = false;
        _cameraFolowing.enabled = true;
    }
    private void Move(bool value)
    {
        _animator.SetBool("Move", value);
    }
}
