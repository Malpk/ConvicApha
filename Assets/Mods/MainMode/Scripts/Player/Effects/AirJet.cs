using UnityEngine;
using PlayerComponent;

namespace MainMode.Items
{
    public class AirJet :Transport
    {
        [Header("General Setting")]
        [Min(1)]
        [SerializeField] private float _timeActive = 1;
        [Min(1)]
        [SerializeField] private float _speedMovement = 1;
        [Min(1)]
        [SerializeField] private float _speedRotation = 1;
        [Header("Requred Reference")]
        [SerializeField] private ParticleSystem _jet;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private FixedJoint2D _fixedPoint;

        private float _progress;
        private Transform _parent;

        public override bool IsActive => _progress < 1f;

        private void Awake()
        {
            _jet.Stop();
        }

        public void Intializate(float timeActive)
        {
            _timeActive = timeActive;
        }

        public override void Enter(Rigidbody2D body)
        {
            _jet.Play();
            enabled = true;
            _fixedPoint.connectedBody = body;
            _progress = 0f;
            _parent = transform.parent;
            transform.parent = null;
        }
        public override void Exit()
        {
            _jet.Stop();
            enabled = false;
            _fixedPoint.connectedBody = null;
            gameObject.SetActive(false);
            transform.parent = _parent;
        }
        public override void Move(Vector2 direction)
        {
            _progress += Time.deltaTime / _timeActive;
            if (direction != Vector2.zero)
                _jet.Play();
            else
                _jet.Stop();
            _rigidbody2D.AddForce(direction * _speedMovement);
            Rotate(direction, _speedRotation);
        }
        private void Rotate(Vector2 direction, float speedRotatoin)
        {
            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, direction), speedRotatoin * Time.deltaTime);
            _rigidbody2D.MoveRotation(rotation);
        }
    }
}