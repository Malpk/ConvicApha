using System.Collections;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public class DeviceTrigerSet : MonoBehaviour
    {
        [Header("General Setting")]
        public bool IsShowTile = false;

        [SerializeField] private Collider2D _collider;
        [SerializeField] private SpriteRenderer _body;
        [SerializeField] private DeviceV2 _device;

        private Coroutine _waitShow;

        public TrapType DeviceType => TrapType.SignalTile;

        public delegate void TrigerEnter(Transform target);
        public event System.Action TrigerEnterAction;
        public event TrigerEnter EnterAction;

        private void Awake()
        {
            _body.enabled = false;
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>())
            {
                if (_device.TryGetComponent(out ISetTarget device))
                    device.SetTarget(collision.transform);
                if (_device.IsShow)
                    _device.Activate();
                else if (_waitShow == null)
                    _waitShow = StartCoroutine(WaitShowAndActivate());
            }
        }

        private IEnumerator WaitShowAndActivate()
        {
            yield return new WaitWhile(() => _device.IsShow);
            _device.Activate();
        }

    }
}