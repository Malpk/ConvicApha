using UnityEngine;
using PlayerComponent;
using UnityEngine.ParticleSystemJobs;

namespace MainMode.Items
{
    public class AirJet : MonoBehaviour, ITransport
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
        private Controller _controller;

        private void Awake()
        {
            _jet.Stop();
        }

        public void Intializate(float timeActive)
        {
            _timeActive = timeActive;
        }

        public void Enter(Player player, Rigidbody2D body, Controller controller)
        {
            enabled = true;
            transform.position = player.transform.position;
            transform.rotation = player.transform.rotation;
            player.Block();
            _fixedPoint.connectedBody = body;
            _jet.Play();
            _user = player;
            if(_controller)
                 _controller.MovementAction -= Move;
            _controller = controller;
            _controller.MovementAction += Move;
        }

        private void Update()
        {
            _progress += Time.deltaTime / _timeActive;
            if (_progress >= 1f)
                Exit();
        }

        public void Exit()
        {
            enabled = false;
            _user.transform.parent = null;
            _user.UnBlock();
            _jet.Stop();
            _controller.MovementAction -= Move;
            _fixedPoint.connectedBody = null;
            gameObject.SetActive(false);
        }
        private void Move(Vector2 direction)
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