using System.Collections;
using UnityEngine;
using MainMode;

namespace MainMode
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class TransSpherePoint : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float _speedFly;
        [SerializeField] private LayerMask _deviceLayer;

        private float _limitDistance = 0.1f;
        private Coroutine _corotine;
        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.enabled = false;
        }

        public void Run(Transform target,Vector2 endPoint)
        {
            if (_corotine == null)
                _corotine = StartCoroutine(MoveSphere(target, endPoint));
        }


        private IEnumerator MoveSphere(Transform target, Vector2 endPoint)
        {
            bool next = true;
            RaycastHit2D hit = new RaycastHit2D();
            while (next && Vector2.Distance(transform.position,endPoint) > _limitDistance)
            {
                hit = Physics2D.CircleCast(transform.position,
                    _collider.radius, transform.up, _speedFly * Time.deltaTime, _deviceLayer);
                if (hit)
                    next = hit.collider.isTrigger;
                transform.position = Vector3.MoveTowards(transform.position, endPoint, _speedFly * Time.deltaTime);
                yield return null;
            }
            _corotine = null;
            target.position = !hit ?  transform.position : (Vector3)hit.point;
            Destroy(gameObject);
        }
    }
}