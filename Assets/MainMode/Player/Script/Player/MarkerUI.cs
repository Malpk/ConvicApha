using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerUI : MonoBehaviour
{
    [SerializeField] private bool _playOnStart;
    [Range(0.1f, 1f)]
    [SerializeField] private float _minVisableOffset = 0.1f;
    [Range(0.1f, 1f)]
    [SerializeField] private float _timeDeactivaMarker = 0.1f;
    [Header("Reference")]
    [SerializeField] private Image _marker;
    [SerializeField] private Player _player;
    [SerializeField] private CameraFollowing _playerCamera;

    private float _previuslyAngle;

    public float CurretAngel { private set; get; }
    public bool IsPlay { private set; get; } = false;

    public void Intilizate(Player player, CameraFollowing playerCamera)
    {
        OnDisable();
        _player = player;
        _playerCamera = playerCamera;
        OnEnable();
    }

    private void OnEnable()
    {
        if(_player)
            _player.DeadAction += () => _marker.enabled = false;
    }
    private void OnDisable()
    {
        if (_player)
            _player.DeadAction -= () => _marker.enabled = false;
    }
    private void Start()
    {
        StartCoroutine(HideMarker());
        if (_playOnStart)
            Play();
    }

    public void Play()
    {
        if (!IsPlay)
        {
            IsPlay = true;
            StartCoroutine(UpdateMarker());
        }
    }
    public void Stop()
    {
        IsPlay = false;
    }

    private IEnumerator UpdateMarker()
    {
        while (IsPlay)
        {
            var localPosition = (Vector2)_playerCamera.Camera.
                ScreenToWorldPoint(Input.mousePosition) - _player.Position;
            CurretAngel = Vector2.Angle(localPosition, Vector2.right);
            CurretAngel *= localPosition.y > 0 ? 1 : -1;
            SetAngle(CurretAngel);
            yield return null;
        }
    }

    public void ShowMarker()
    {
        _marker.enabled = true;
    }
    private IEnumerator HideMarker()
    {
        while (true)
        {
            if (Mathf.Abs(_previuslyAngle - CurretAngel) < _minVisableOffset)
            {
                yield return Hide();
            }
            _previuslyAngle = CurretAngel;
        }
    }
    private IEnumerator Hide()
    {
        var progress = 0f;
        while (Mathf.Abs(_previuslyAngle - CurretAngel) < _minVisableOffset && _timeDeactivaMarker >= progress)
        {
            progress += Time.deltaTime / _timeDeactivaMarker;
            yield return null;
            _previuslyAngle = CurretAngel;
        }
        _marker.enabled = progress < _timeDeactivaMarker && !_player.IsDead;
        yield return new WaitWhile(() =>
        {
            if (Mathf.Abs(_previuslyAngle - CurretAngel) < _minVisableOffset)
            {
                _previuslyAngle = CurretAngel;
                return true && !_marker.enabled;
            }
            return false;
        });
        _marker.enabled = true;
    }
    private void SetAngle(float angle)
    {
        transform.localRotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
