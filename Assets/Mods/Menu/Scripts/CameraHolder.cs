using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private int _translationSpeed;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _bannerHolder;

    private Vector3 _startPos;
    private ModeBanner[] _banners;

    private void OnValidate()
    {
        if (_bannerHolder)
        {
            _banners = _bannerHolder.
                GetComponentsInChildren<ModeBanner>();
        }
    }

    private void OnEnable()
    {
        foreach (var banner in _banners)
        {
            banner.OnSelect += MoveCamera;
        }
    }

    private void OnDisable()
    {
        foreach (var banner in _banners)
        {
            banner.OnSelect -= MoveCamera;
        }
    }
    private void Update()
    {
        _camera.transform.Translate(Vector3.right * Time.deltaTime * _translationSpeed * 0.1f);
    }
    private void MoveCamera(Vector3 position)
    {
        transform.position = position;
        _camera.transform.localPosition = Vector3.zero;
    }
}
