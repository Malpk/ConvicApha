using System.Collections;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class DetecDevice : MonoBehaviour, IDetectMode, IPause
    {
        [Header("Pefab Setting")]
        [SerializeField] private LayerMask _playerLayer;

        private bool _mode = false;
        private bool _isPause = false;
        private Vector3[] _cheakPoint = new Vector3[]
        {
            Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.zero,
            new Vector3(1,1),  new Vector3(-1,1),  new Vector3(1,-1),  new Vector3(-1,-1)
        };
        private BoxCollider2D _boxCollider;
        private Coroutine _coroutine = null;

        public abstract TileState state { get; }

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _boxCollider.isTrigger = true;
            Intializate();
        }
        protected abstract void Intializate();

        protected abstract void Damage(IDamage player);

        public void Pause()
        {
            _isPause = false;
        }

        public void UnPause()
        {
            _isPause = true;
        }

        public void SetTrackingMode(bool mode)
        {
            _mode = mode;
            if(_coroutine == null && gameObject.activeSelf)
                _coroutine = StartCoroutine(Tracking());
            return;
        }
        private IEnumerator Tracking()
        {
            while (_mode)
            {
                yield return new WaitWhile(() => _isPause);
                foreach (var offset in _cheakPoint)
                {
                    var hitPosition = transform.position + offset * _boxCollider.size.x / 2;
                    Cheak(hitPosition);
                }
                _coroutine = null;
                yield return null;
            }
        }
        private void Cheak(Vector3 hitPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(hitPosition, Vector3.forward, 2f, _playerLayer);
#if UNITY_EDITOR
            Debug.DrawLine(hitPosition, hitPosition + Vector3.forward * 2f,Color.green);
#endif
            if (hit)
            {
                if (hit.transform.TryGetComponent<IDamage>(out IDamage player))
                {
                    Damage(player);
                }
            }
        }


    }

}