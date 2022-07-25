using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Underworld;

namespace Underworld
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(AudioSource))]
    public class PlayerLegacy : MonoBehaviour
    {
        [Header("Game Setting")]
        [SerializeField] private float _speedMovement = 7f;
        [SerializeField] private float _speedRotation;
        [Header("Perfab Setting")]
        [SerializeField] private AudioClip _deadSound;

        private bool _immortalityMode = false;
        private Animator _animator;
        private AudioSource _soundSource;
        private LvlSate _curretState = LvlSate.Play;

        public delegate void Dead();
        public event Dead DeadAction;

        public Vector3 Position => transform.position;

        private void Awake()
        {
            _soundSource = GetComponent<AudioSource>();
            _soundSource.playOnAwake = false;
            _soundSource.loop = false;
            _soundSource.clip = _deadSound;
            _animator = GetComponent<Animator>();
            var rigidbody = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
           
        }

   
        private void DeadPlayer()
        {
            Time.timeScale = 0f;
            _soundSource.Stop();
            if (DeadAction != null)
                DeadAction();
        }
        private void SetPause()
        {
            _curretState = LvlSate.Pause;
        }
    }
}