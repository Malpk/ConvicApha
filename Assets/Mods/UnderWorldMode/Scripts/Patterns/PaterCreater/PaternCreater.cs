using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public sealed class PaternCreater : TotalMapMode
    {
        [Header("Game setting")]
        [SerializeField] private bool _iversionMode;
        [SerializeField] private float _warningTime;
        [SerializeField] private Color _deffaout;
        [SerializeField] private Vector2Int _unitySprite;
        [SerializeField] private Texture2D _spriteAtlas;

        private float _errorColorDefaout;
        private Color deactiveColor;
        private BasePatternState _curretState;
        private List<Term> _frame;
        private PatternIdleState _warningState;
        private PatternCreaterParceState _parceState;

        public bool IsActive { get; private set; } = false;

        protected override void Awake()
        {
            base.Awake();
            var countOffset = GetCountOffset();
            _warningState = new PatternIdleState(_warningTime);
            _parceState = new PatternCreaterParceState(workDuration / countOffset.x, countOffset);
            _warningState.SetNextState(_parceState);
            _parceState.SetNextState(compliteState);
            deactiveColor = _iversionMode ? Color.black : Color.white;
            enabled = false;
        }
        public override void SetConfig(PaternConfig config)
        {
            if (config is PaternCreaterConfig paternCreaterConfig)
            {
                _iversionMode = paternCreaterConfig.InversMode;
                _warningTime = paternCreaterConfig.WarningTime;
                workDuration = paternCreaterConfig.WorkDuration;
            }
            else
            {
                throw new System.NullReferenceException("PaternCreaterConfig is null");
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _warningState.OnComplite += () => ActivateTerms(_frame);
            _parceState.OnUpdateFrame += UpdateFrame;
            _parceState.OnComplite += DeactivateTerms;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _warningState.OnComplite -= () => ActivateTerms(_frame);
            _parceState.OnUpdateFrame -= UpdateFrame;
            _parceState.OnComplite -= DeactivateTerms;
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
        public override bool Play()
        {
            if (!enabled)
            {
                enabled = true;
                _frame = GetFrame(_spriteAtlas, new Vector2Int(_unitySprite.x, 0)).ToList();
                _curretState = _warningState;
                foreach (var term in _frame)
                {
                    term.Show();
                }
                return true;
            }
            return false;
        }
        public void Stop()
        {
            enabled = false;
        }
        private void UpdateFrame(Vector2Int position)
        {
            var curretPosition = new Vector2Int(position.x * _unitySprite.y, position.y * _unitySprite.y);
            var curretFrame = GetFrame(_spriteAtlas, curretPosition).ToList();
            DectivateTerms(curretFrame, _frame);
            _frame = curretFrame;
            ActivateTerms(_frame);
        }
        private void DectivateTerms(List<Term> curretFrame, List<Term> previousFrame)
        {
            for (int i = 0; i < previousFrame.Count; i++)
            {
                if (!curretFrame.Contains(previousFrame[i]))
                {
                    previousFrame[i].Deactivate(false);
                    previousFrame[i].Hide();
                }
            }
        }
        #region Pacing Sprite
        private IEnumerable<Term> GetFrame(Texture2D texture, Vector2Int startPosition)
        {
            for (int i = 0; i < _unitySprite.x; i++)
            {
                var indexI = startPosition.x + i;
                for (int j = 0; j < _unitySprite.y; j++)
                {
                    var inkecJ = startPosition.y + j;
                    var color = texture.GetPixel(indexI, inkecJ);
                    if (DefineState(color, new Vector2Int(i, j), out Term term))
                    {
                        yield return term;
                    }
                }
            }
        }
        private bool DefineState(Color color, Vector2Int termPosition,out Term term)
        {
            term = null;
            if (color == deactiveColor)
            {
                if (!Equale(color, _deffaout, _errorColorDefaout))
                {
                    term = base.terms[termPosition.x, termPosition.y];
                }
            }
            return term;
        }
        #endregion
        private bool Equale(Color to, Color from, float errorFiltr = 0f)
        {
            var b = Mathf.Abs(to.b - from.b);
            var g = Mathf.Abs(to.g - from.g);
            var r = Mathf.Abs(to.r - from.r);
            return r < errorFiltr && b < errorFiltr && g < errorFiltr;
        }

        private Vector2Int GetCountOffset()
        {
            var x = _spriteAtlas.width / _unitySprite.x;
            var y = _spriteAtlas.height / _unitySprite.y;
            return new Vector2Int(x, y);
        }
    }
}