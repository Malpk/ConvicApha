using UnityEngine;

namespace PlayerComponent
{
    [System.Serializable]
    public class PlayerMovementSet : MonoBehaviour, IPlayerComponent
    {
        [SerializeField] private PlayerStats _state;
        [Header("Reference")]
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private PlayerEffectSet _effectSet;

        private Vector3 _startPosition;

        private Transport _seedTransport;

        private System.Action<Vector2> MoveState;

        public bool IsSeedToTransport => _seedTransport;

        private void Awake()
        {
            _startPosition = transform.position;
        }

        public void Play()
        {
            MoveState = Moving;
            transform.position = _startPosition;
        }
        public void EnterToTransport(Transport transport)
        {
            _seedTransport = transport;
            transport.Enter(_rigidBody);
            MoveState = MoveOnTransport;
        }
        public void ExitToTransport()
        {
            if (_seedTransport != null)
                _seedTransport.Exit();
            _seedTransport = null;
            transform.parent = null;
            MoveState = Moving;
        }
        public void Move(Vector2 input)
        {
            MoveState(input);
        }
        public void MoveToPosition(Vector2 position)
        {
            if (!_seedTransport)
            {
                _rigidBody.MovePosition(position);
            }
        }

        public void Stop()
        {
            _rigidBody.velocity = Vector2.zero;
            ExitToTransport();
        }

        private void MoveOnTransport(Vector2 input)
        {
            if (_seedTransport.IsActive)
                _seedTransport.Move(input);
            else
                ExitToTransport();
        }
        private void Moving(Vector2 input)
        {
            if (input == Vector2.zero)
                return;
            _rigidBody.AddForce(input * _state.SpeedMovement * _effectSet.MoveEffect, ForceMode2D.Force);
            _rigidBody.MoveRotation(Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(Vector3.forward, input), _state.SpeedMovement * _effectSet.MoveEffect * Time.deltaTime));
        }

        public void ResetState()
        {
            ExitToTransport();
        }
    }
}