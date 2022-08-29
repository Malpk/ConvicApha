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

        private bool _isDeactivate;

        public FireState CurretState { get; private set; } = FireState.End;

        private void Awake()
        {
            if(!_playOnAwake)
                DeactiveEvent();
        }

        public bool Activate(FireState state)
        {
#if UNITY_EDITOR
            if (CurretState == state)
                throw new System.Exception("Fire is already activation");
            else if (state == FireState.End)
                return false;
#endif
            CurretState = state;
            _body.enabled = true;
            _animator.SetInteger("State", GetState(state));
            return true;
        }
        public void DeactiveEvent()
        {
#if UNITY_EDITOR
            if (CurretState == FireState.End)
            {
                throw new System.Exception("Fire is already deactivation");
            }
#endif
            _isDeactivate = false;
            _body.enabled = false;
            CurretState = FireState.End;
            _animator.SetInteger("State", 0);
        }
        public void DeactivateWaitAnimation()
        {
#if UNITY_EDITOR
            if(_isDeactivate && CurretState == FireState.End)
            {
                throw new System.Exception("Fire is already deactivation animation");
            }
#endif
            _isDeactivate = true;
            _animator.SetInteger("State", GetState(FireState.End));
        }
        private int GetState(FireState state)
        {
            switch (state)
            {
                case FireState.Start:
                    return 1;
                case FireState.Stay:
                    return 2;
                default:
                    return 3;
            }
        }
    }
}