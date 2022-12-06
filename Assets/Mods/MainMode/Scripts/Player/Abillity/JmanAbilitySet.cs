using MainMode.GameInteface;
using UnityEngine;

namespace PlayerComponent
{
    public class JmanAbilitySet : AbillityActiveSet
    {
        [Header("Ability")]
        [Min(0.1f)]
        [SerializeField] private float _duration = 1;
        [Min(1)]
        [SerializeField] private float _distance = 1;
        [SerializeField] private float _timeActiveDebaf;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private MovementEffect _debaf;
        [SerializeField] private AnimationCurve _jerkCurve;
        [Header("Reference")]
        [SerializeField] private Collider2D _playerBody;
        
        private Vector2 _startPosition;
        private Vector2 _jerkForce;

        protected override void UseAbility()
        {
            user.Block();
            _progress = 0f;
            _playerBody.enabled = false;
            SetReloadState(true);
            enabled = true;
            _jerkForce = user.transform.up * _distance;
            _startPosition = user.transform.position;
            State = JerkUpdate;
            user.Invulnerability(true);
        }
        private void JerkUpdate()
        {
            _progress += Time.fixedDeltaTime / _duration;
            var move = _startPosition + _jerkForce * _jerkCurve.Evaluate(_progress);
            var hit = Physics2D.Raycast(user.transform.position, user.transform.up, 
                Vector2.Distance(user.transform.position, move), _wallLayer);
            if (_progress >= 1f || hit)
            {
                _progress = 0f;
                IsActive = false;
                user.UnBlock();
                State = Reloading;
                user.GetComponent<PlayerEffectSet>().AddEffects(_debaf, _timeActiveDebaf);
                _playerBody.enabled = true;
                user.Invulnerability(false);
            }
            else
            {
                user.MoveToPosition(move);
            }
        }
    }
}
