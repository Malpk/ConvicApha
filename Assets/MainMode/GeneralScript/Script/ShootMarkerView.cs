using UnityEngine;
using Zenject;

namespace MainMode.GameInteface
{
    public class ShootMarkerView : MonoBehaviour
    {
        [Header("General Setting")]
        [Range(0.1f, 1f)]
        [SerializeField] private float _timeActive = 0.1f;
        [Range(0.1f, 1f)]
        [SerializeField] private float _minVisableOffset = 0.1f;
        [Header("Reference")]
        [SerializeField] private Transform _markerHolder;


        private float _previuslyAngle;
        [SerializeField] private Camera _camera;
        private ShowMarkerState _shoowState;

        public float Angel { get; private set; } = 0;

        private void Awake()
        {
            _shoowState = new ShowMarkerState(_markerHolder);
        }
        [Inject]
        public void Construct(CameraFollowing camera)
        {
            _camera = camera.Camera;
        }
        public void Update()
        {
            _previuslyAngle = Angel;
            Angel = GetAngle();
            transform.rotation = Quaternion.Euler(Vector3.forward * Angel);
            if (Mathf.Abs(Angel - _previuslyAngle) >= _minVisableOffset)
            {
                _shoowState.Start(_timeActive);
            }
            _shoowState.Update();
        }

        private float GetAngle()
        {
            var localPosition = (Vector2)(_camera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            return Vector2.Angle(localPosition, Vector2.right) * (localPosition.y > 0 ? 1 : -1);
        }
    }
}