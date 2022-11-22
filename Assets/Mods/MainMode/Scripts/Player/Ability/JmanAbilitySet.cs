using MainMode.GameInteface;
using UnityEngine;

namespace PlayerComponent
{
    public class JmanAbilitySet : PlayerAbillityUseSet
    {
        [Header("Ability")]
        [Min(0.1f)]
        [SerializeField] private float _duration = 1;
        [Min(1)]
        [SerializeField] private float _distance = 1;
        [SerializeField] private float _timeActiveDebaf;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private Sprite _abillityIcon;
        [SerializeField] private MovementEffect _debaf;
        [SerializeField] private AnimationCurve _jerkCurve;

        private Vector2 _startPosition;
        private Vector2 _jerkForce;

        public System.Action State;

        private void Awake()
        {
            enabled = false; 
        }
        public override void SetHud(HUDUI hud)
        {
            base.SetHud(hud);
            hud.SetAbilityIcon(_abillityIcon);
        }
        private void FixedUpdate()
        {
            State();
        }

        protected override void UseAbility()
        {
            user.Block();
            _progress = 0f;
            SetReloadState(true);
            hud.DisplayStateAbillity(false);
                enabled = true;
            _jerkForce = user.transform.up * _distance;
            _startPosition = user.transform.position;
            State = JerkUpdate;
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
                State = ReloadUpdate;
                user.AddEffects(_debaf, _timeActiveDebaf);
            }
            else
            {
                user.MoveToPosition(move);
            }
        }


    }
}
