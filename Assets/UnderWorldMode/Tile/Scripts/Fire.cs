using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Underworld
{
    public class Fire : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnAwake;
        [Header("Requred Setting")]
        [SerializeField] private SpriteRenderer _body;
        [SerializeField] private Animator _animator;

        public bool IsActive => _body.enabled;

        private void Awake()
        {
            if(!_playOnAwake)
                Hide();
        }

        public bool Activate(FireState state)
        {
            if (state == FireState.End)
                return false;
            _body.enabled = true;
            _animator.SetInteger("State", GetState(state));
            return true;
        }
        public void Diactivate()
        {
            _animator.SetInteger("State", GetState(FireState.End));
        }
        private void Hide()
        {
            _body.enabled = false;
            _animator.SetInteger("State", 0);
        }
        private int GetState(FireState state)
        {
            switch (state)
            {
                case FireState.Start:
                    return 1;
                case FireState.Stay:
                    return 2;
                case FireState.End:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}