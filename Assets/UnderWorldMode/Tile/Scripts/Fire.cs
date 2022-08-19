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

        public FireState CurretState { get; private set; }

        private void Awake()
        {
            if(!_playOnAwake)
                Deactive();
        }

        public bool Activate(FireState state)
        {
            if (state == FireState.End)
                return false;
            CurretState = state;
            _body.enabled = true;
            _animator.SetInteger("State", GetState(state));
            return true;
        }
        public void Deactive()
        {
            _body.enabled = false;
            CurretState = FireState.End;
            _animator.SetInteger("State", 0);
        }
        public void DeactivateWaitAnimation()
        {
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