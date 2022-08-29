using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace Underworld
{
    public sealed class PaternCreater : TotalMapMode
    {
        [Header("Game setting")]
        [SerializeField] private bool _iversionMode;
        [SerializeField] private float _warningTime;
        [SerializeField] private float _errorColorDefaout;
        [SerializeField] private Color _deffaout;
        [SerializeField] private Vector2Int _unitySprite;
        [SerializeField] private Texture2D _spriteAtlas;

        private Color deactiveColor;
        private Coroutine _runMode = null;

        public bool IsActive { get; private set; } = false;

        private void Awake()
        {
            deactiveColor = _iversionMode ? Color.black : Color.white;
        }
        public override bool Activate()
        {
            if (_runMode == null)
            {
                State = ModeState.Play;
                _runMode = StartCoroutine(StartMode());
                return true;
            }
            return false;
        }
        private IEnumerator StartMode()
        {
            State = ModeState.Play;
            yield return new WaitWhile(() => !IsReady);
            var firstFrame = GetFrame(_spriteAtlas, Vector2Int.zero).ToList();
            foreach (var term in firstFrame)
            {
                term.ShowItem();
            }
            yield return WaitTime(_warningTime);
            ActivateTerms(firstFrame);
            yield return ReadAnimation(firstFrame);
            State = ModeState.Stop;
            _runMode = null;
        }
        private IEnumerator ReadAnimation(List<Term> previousFrame)
        {
            var countOffset = GetCountOffset();
            var delay = (workDuration / countOffset.x);
            for (int i = 0; i < countOffset.y; i++)
            {
                for (int j = 0; j < countOffset.x && State != ModeState.Stop; j++)
                {
                    var curretPosition = new Vector2Int(i * _unitySprite.y, j * _unitySprite.y);
                    var curretFrame = GetFrame(_spriteAtlas, curretPosition).ToList();
                    var getDeactivateTermTask = Task.Run(() => GetPreviusTils(curretFrame, previousFrame));
                    var getActivateTermTask = Task.Run(() => GetCurretTiles(curretFrame));
                    yield return WaitTime(delay);
                    yield return new WaitWhile(() => !(getDeactivateTermTask.IsCompleted && getActivateTermTask.IsCompleted));
                    ActivateTerms(getActivateTermTask.Result);
                    yield return delay;
                    DectivateTerms(getDeactivateTermTask.Result);
                    previousFrame = curretFrame;
                }
            }
            yield return WaitTime(1f);
            yield return WaitHideMap();
            State = ModeState.Stop;
        }
        private void ActivateTerms(List<Term> terms)
        {
            foreach (var term in terms)
            {
                if(!term.IsShow)
                    term.ShowItem();
                term.Activate(FireState.Start);
            }
        }
        private void DectivateTerms(List<Term> terms)
        {
            foreach (var term in terms)
            {
                term.Deactivate();
                term.StartCoroutine(term.HideByDeactivation());
            }
        }
        private List<Term> GetPreviusTils(List<Term> curretFrame, List<Term> previousFrame)
        {
            var list = new List<Term>();
            for (int i = 0; i < previousFrame.Count; i++)
            {
                if (!curretFrame.Contains(previousFrame[i]))
                {
                    list.Add(previousFrame[i]);
                }
            }
            return list;
        }
        private List<Term> GetCurretTiles(List<Term> frame)
        {
            var list = new List<Term>();
            for (int i = 0; i < frame.Count; i++)
            {
                if (!frame[i].IsDamageMode)
                {
                    list.Add(frame[i]);
                }
            }
            return list;
        }
        #region Pacing Sprite
        private IEnumerable<Term> GetFrame(Texture2D texture, Vector2Int startPosition)
        {
            for (int i = 0; i < _unitySprite.y; i++)
            {
                var indexI = startPosition.y + i;
                for (int j = 0; j < _unitySprite.x; j++)
                {
                    var inkecJ = startPosition.x + j;
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
                    term = termArray[termPosition.x, termPosition.y];
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