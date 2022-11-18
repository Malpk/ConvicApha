using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxMode : TotalMapMode
    {
        [Header("Movement Setting")] [Min(1)]
        [SerializeField] private int _countMove;
        [Min(0)]
        [SerializeField] private float _speedMovement;
        [Header("Time Setting")] [Min(1)]
        [SerializeField] private float _scaleDuration;
        [Min(0)]
        [SerializeField] private float _delayMove;
        [Header("Scale Setting")]
        [SerializeField] private Vector2 _minSize;
        [SerializeField] private Vector2 _maxOffset;

        private PatternLinerInterplate _scaleState;
        private BoxModeMoveState _moveState;

        private Vector2 _target;
        private Vector2 _startSize;
        private BasePatternState _curretState;
        private BoxCollider2D _collider;

        public bool IsPlay => enabled;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<BoxCollider2D>();
            _collider.enabled = false;
            _startSize = _collider.size;
            _maxOffset -= Vector2.one * _minSize / 2;
            IntializateStateMachine();
            enabled = false;
        }
        private void IntializateStateMachine()
        {
            _scaleState = new PatternLinerInterplate(_scaleDuration);
            var idle = new PatternIdleState(_delayMove);
            _moveState = new BoxModeMoveState(transform, _maxOffset, _countMove, _speedMovement);
            _scaleState.SetNextState(idle);
            idle.SetNextState(_moveState);
            _moveState.SetNextState(compliteState);
        }
        public override void SetConfig(PaternConfig config)
        {
            if (config is BoxModeConfig boxModeConfig)
            {
                _countMove = boxModeConfig.CountMove;
                _delayMove = boxModeConfig.DealyMove;
                _scaleDuration = boxModeConfig.ScaleTime;
                workDuration = boxModeConfig.WorkDuration;
            }
            else
            {
                throw new System.NullReferenceException("BoxModeConfig is null");
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _scaleState.OnUpdate += ScaleBox;
            _moveState.OnComplite += DeactivateTerms;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _scaleState.OnUpdate -= ScaleBox;
            _moveState.OnComplite -= DeactivateTerms;
        }

        public override bool Play()
        {
            enabled = true;
            _collider.enabled = true;
            _collider.size = _startSize;
            transform.position = Vector2.zero;
            _target = GetOffset(_maxOffset);
            _curretState = _scaleState;
            _curretState.Start();
            return true;
        }
        public void Stop()
        {
            enabled = false;
            _collider.enabled = false;
        }
        private void Update()
        {
            if (_curretState.IsComplite)
            {
                if (_curretState.GetNextState(out BasePatternState nextState))
                {
                    _curretState = nextState;
                    _curretState.Start();
                }
                else
                {
                    Stop();
                }
            }
            else
            {
                _curretState.Update();
            }
        }

        private void ScaleBox(float progress)
        {
            _collider.size = Vector2.Lerp(_startSize, _minSize, progress);
            transform.position = Vector2.Lerp(transform.position, _target, progress);
        }
        private Vector3 GetOffset(Vector3 offset)
        {
            var x = GetOffset(offset.x);
            var y = GetOffset(offset.y);
            return new Vector3(x, y, 0);
        }
        private float GetOffset(float offset)
        {
            return Random.Range(-offset, offset);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Term term))
            {
                if (term.IsActive)
                {
                    term.Deactivate(false);
                    term.Hide();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Term term) && _collider.enabled)
            {
                if (!term.IsActive)
                {
                    term.Show();
                    term.Activate(FireState.Stay);
                }
            }
        }

    }
}