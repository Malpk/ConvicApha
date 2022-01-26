using System.Collections;
using UnityEngine;
using GameMode;
using Zenject;

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
        _gameEvent.StatusUpdate += Deactive;
    }
    private void OnDisable()
    {
        _gameEvent.StatusUpdate -= Deactive;
    }
    private void Start()
    {
        var value = _gameEvent.state == GameState.Play;
        Move(!value);
    }
    private void Deactive(GameState state)
    {
        if (state != GameState.Play)
            return;
        if (CompliteAction != null)
            CompliteAction();
        Move(false);
        enabled = false;
        _cameraFolowing.enabled = true;
        Debug.Log("Complite");
    }
    private void Move(bool value)
    {
        _animator.SetBool("Move", value);
    }
}
