using UnityEngine;
using UnityEngine.Events;

namespace PlayerComponent
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private PlayerState _previousState;
        private System.Action<bool> _previousAnimation;

        public void SetAnimation(PlayerState state, bool mode)
        {
            if (_previousState != state)
            {
                _previousAnimation?.Invoke(false);
                GetStateName(state, mode);
                _previousState = state;
            }
            else
            {
                _previousAnimation?.Invoke(mode);
            }
        }
        public void ResetAnimator()
        {
            _previousAnimation?.Invoke(false);
        }
        private void GetStateName(PlayerState state, bool mode)
        {
            switch (state)
            {
                case PlayerState.Freez:
                    Freez(mode);
                    _previousAnimation = Freez;
                    break;
                case PlayerState.Dead:
                    Dead(mode);
                    _previousAnimation = Dead;
                    break;
                case PlayerState.Invulnerability:
                    Invulnerability(mode);
                    _previousAnimation = Invulnerability;
                    break;
                default:
                    break;
            }
        }
        private void Invulnerability(bool mode)
        {
            _animator.SetBool("Invulnerability", mode);
        }
        private void Dead(bool mode)
        {
            _animator.SetBool("Dead", mode);
        }
        private void Freez(bool mode)
        {
            _animator.SetBool("Freez", mode);
        }
    }
}