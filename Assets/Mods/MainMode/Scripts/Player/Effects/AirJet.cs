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
        private Player _user;
        private Transform _parent;
        private PCPlayerController _controller;

        private void Awake()
        {
            _jet.Stop();
        }

        public void Intializate(float timeActive)
        {
            _timeActive = timeActive;
        }

        public override void Enter(Player player, Rigidbody2D body, PCPlayerController controller)
        {
            enabled = true;
            _controller = controller;
            _controller.SetMovement(this);
            _fixedPoint.connectedBody = body;
            _jet.Play();
            _user = player;
            _parent = transform.parent;
            transform.parent = null;
        }
        public override void Exit()
        {
            enabled = false;
            _jet.Stop();
            _user.ExitToTransport();
            _controller.SetMovement(_user);
            _fixedPoint.connectedBody = null;
            gameObject.SetActive(false);
            transform.parent = _parent;
        }
        private void Update()
        {
            _progress += Time.deltaTime / _timeActive;
            if (_progress >= 1f)
                Exit();
        }
        public override void Move(Vector2 direction)
        {
            if (direction != Vector2.zero)
                _jet.Play();
            else
                _jet.Stop();
            _rigidbody2D.AddForce(direction * _speedMovement * _user.MovementEffect);
            Rotate(direction, _speedRotation);
        }
        private void Rotate(Vector2 direction, float speedRotatoin)
        {
            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, direction), speedRotatoin * Time.deltaTime);
            _rigidbody2D.MoveRotation(rotation);
        }
    }
}