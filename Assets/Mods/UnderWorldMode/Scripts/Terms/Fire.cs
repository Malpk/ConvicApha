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

        public System.Action OnDeactivateFire;

        public FireState CurretState { get; private set; } = FireState.End;

        private void Awake()
        {
            if(!_playOnAwake)
                DeactiveEvent();
        }

        public bool Activate(FireState state)
        {
            CurretState = state;
            _body.enabled = true;
            _animator.SetInteger("State", GetState(state));
            return true;
        }
        public void Deactivete()
        {
            _body.enabled = false;
            CurretState = FireState.End;
            _animator.SetInteger("State", 0);
        }
        public void DeactivateWaitAnimation()
        {
            _animator.SetInteger("State", GetState(FireState.End));
        }
        private void DeactiveEvent()
        {
            Deactivete();
            OnDeactivateFire?.Invoke();
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