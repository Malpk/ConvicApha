using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMode;
using Zenject;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] private float _speedMovement = 1f;
    [SerializeField] private float _returnCamera = 1f;
    [Header("Perfab Setting")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _mapSize;
    [SerializeField] private CameraFolowing _cameraFolowing;
    [SerializeField] private Transform _player;

    [Inject] private GameEvent _gameEvent;

    private Vector2 _maxOffset;
    private Vector3[] _directions = new Vector3[]
    {
       new Vector3 (-1,-1),new Vector3 (1,-1),
       new Vector3(1,1), new Vector3(-1,1) 
    };

    public delegate void Complite();
    public Complite CompliteAction;

    private void Start()
    {
        if (_gameEvent.state == GameState.Pause)
        {
            var cameraSize = GetCameraSize(_camera);
            _maxOffset = GetOffset(cameraSize, _mapSize);
            StartCoroutine(RunAnimation());
        }
        else
        {
            var target = new Vector3(_player.position.x, _player.position.y, transform.position.z);
            transform.position = target;
            Deactive();
        }
    }
    private Vector2 GetOffset(Vector2 cameraSize, Vector2 mapSize)
    {
        var x = Mathf.Abs(cameraSize.x - mapSize.x);
        var y = Mathf.Abs(cameraSize.y - mapSize.y);
        return new Vector2(x, y);
    }
    private Vector2 GetCameraSize(Camera camera)
    {
        var cameraSize = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight));
        cameraSize -= camera.transform.position;
        return new Vector2(Mathf.Abs(cameraSize.x), Mathf.Abs(cameraSize.y));
    }
    private IEnumerator RunAnimation()
    {
        while (_gameEvent.state == GameState.Pause)
        {
            yield return StartCoroutine(Move(_directions,_maxOffset, _speedMovement));
            yield return null;
        }
        var target = new Vector3(_player.position.x, _player.position.y, transform.position.z);
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _returnCamera * Time.deltaTime);
            yield return null;
        }
        Deactive();
    }
    private void Deactive()
    {
        if (CompliteAction != null)
            CompliteAction();
        enabled = false;
        _cameraFolowing.enabled = true;
    }
    private IEnumerator Move(Vector3 [] directions,Vector2 offset, float speed = 1f)
    {
        for (int i = 0; i < directions.Length && _gameEvent.state == GameState.Pause; i++)
        {
            var target = new Vector3(directions[i].x * offset.x, directions[i].y * offset.y, transform.position.z);
            while (target != transform.position && _gameEvent.state == GameState.Pause)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
