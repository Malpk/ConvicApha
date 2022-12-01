using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class TransSpherePoint : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float _speedFly;
        [SerializeField] private LayerMask _deviceLayer;
        [SerializeField] private Animator _animator;
        [SerializeField] private TpStartPoint _tpPoint;

        private float _limitDistance = 0.1f;
        private Vector3 _target;
        private RaycastHit2D _hit;
        private Transform _user;
        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.enabled = false;
            _tpPoint.OnComplite += Apperance;
            enabled = false;
        }
        private void OnDestroy()
        {
            _tpPoint.OnComplite -= Apperance;
        }

        public void Run(Transform user,Vector2 endPoint)
        {
            enabled = true;
            _target = endPoint;
            _user = user;
            _tpPoint.transform.parent = null;
            _tpPoint.transform.position = transform.position;
        }

        private void Update()
        {
            _hit = Physics2D.CircleCast(transform.position, _collider.radius,
                transform.up, _speedFly * Time.deltaTime, _deviceLayer);
            if (Vector2.Distance(transform.position, _target) > _limitDistance && !_hit)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target, _speedFly * Time.deltaTime);
            }
            else
            {
                _tpPoint.gameObject.SetActive(true);
            }
        }

        private void Apperance()
        {
            _user.gameObject.SetActive(false);
            _animator.SetBool("Tp" ,true);
        }
        private void CompliteEvent()
        {
            _user.gameObject.SetActive(true);
            _user.position = !_hit ? transform.position : (Vector3)_hit.point;
        }
        private void DestroyAnimationEvent()
        {
            Destroy(gameObject);
            Destroy(_tpPoint.gameObject);
        }
    }
}