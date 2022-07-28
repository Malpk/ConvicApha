using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using UnityEngine.ParticleSystemJobs;
using MainMode.Effects;

namespace MainMode.Items
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AirJet : MonoBehaviour, ITransport
    {
        [Header("General Setting")]
        [Min(1)]
        [SerializeField] private float _speedMovement;
        [Min(1)]
        [SerializeField] private float _speedRotation;
        [Header("Requred Reference")]
        [SerializeField] private ParticleSystem _jet;

        private Rigidbody2D _rigidbody2D;
        private Joint2D _fixedJoint;

        private void Awake()
        {
            _jet.Stop();
            _fixedJoint = GetComponent<Joint2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Enter(Rigidbody2D target)
        {
            _fixedJoint.connectedBody = target;
        }

        public void Exit()
        {
            _fixedJoint.connectedBody = null;
            Destroy(gameObject);
        }
        public void Move(Vector2 direction)
        {
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