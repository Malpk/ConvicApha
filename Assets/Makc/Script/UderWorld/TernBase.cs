using System.Collections;
using UnityEngine;
using PlayerComponent;

namespace Underworld
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class TernBase : MonoBehaviour, IDetectMode
    {
        [Header("Pefab Setting")]
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] protected Vector3 fireOffset;

        private bool _mode = false;
        private Vector3[] _offset = new Vector3[]
        {
            Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.zero,
            new Vector3(1,1),  new Vector3(-1,1),  new Vector3(1,-1),  new Vector3(-1,-1)
        };
        private Vector3 _sizeCollider;
        private Coroutine _coroutine = null;

        public TypeTile tileType => TypeTile.TernTile;
        public abstract TernState state { get; }

        private void Awake()
        {
            _sizeCollider = GetComponent<BoxCollider2D>().size;
            Intializate();
        }
        protected abstract void Intializate();
        protected GameObject InstatiateFire(GameObject fire)
        {
            var fireInstatiate = Instantiate(fire, transform);
            fireInstatiate.transform.parent = transform;
            fireInstatiate.transform.localPosition = fireOffset;
            return fireInstatiate;
        }
        protected abstract IEnumerator Work();

        protected abstract void Damage(Player player);

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
                foreach (var offset in _offset)
                {
                    var hitPosition = transform.position + offset * _sizeCollider.x/2;
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
                if (hit.transform.TryGetComponent<Player>(out Player player))
                {
                    Damage(player);
                }
            }
        }
    }

}