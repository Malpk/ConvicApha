using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public class PaternCreater : TotalMapMode
    {
        [Header("Game setting")]
        [SerializeField] protected bool iversionMode;
        [SerializeField] protected float errorColorDefaout;
        [SerializeField] protected Color _deffaout;
        [SerializeField] protected Vector2Int _unitySprite;
        [SerializeField] protected Sprite _spriteAtlas;
       
        protected int oneSecond = 1;
        protected Color deactiveColor;
        protected Point[,] _map;
        protected Coroutine _runMode = null;

        public bool IsAttackMode => _runMode != null;

        protected override void Awake()
        {
            base.Awake();
            deactiveColor = iversionMode ? Color.black : Color.white;
        }
        public override bool Activate()
        {
            if (_runMode == null && IsReady)
            {
                State = ModeState.Play;
                _runMode = StartCoroutine(RunPatern());
                return true;
            }
            return false;
        }
        private IEnumerator RunPatern()
        {
            var countOffset = GetCountOffset();
            List<HandTermTile> previousFrame = null;
            for (int i = 0; i < countOffset.y; i++)
            {
                for (int j = 0; j < countOffset.x && State != ModeState.Stop; j++)
                {
                    var curretPosition = new Vector2Int(i * _unitySprite.y, j * _unitySprite.y);
                    var curretFrame = ReadTexture(_spriteAtlas.texture, curretPosition).ToList();
                    DeactivePreviusTils(curretFrame, previousFrame);
                    previousFrame = curretFrame;
                    yield return WaitTime((workDuration / countOffset.x) + oneSecond);
                }
            }
            State = ModeState.Stop;
            _runMode = null;
        }
        #region Pacing Sprite
        private IEnumerable<HandTermTile> ReadTexture(Texture2D texture, Vector2Int startPosition)
        {
            for (int i = 0; i < _unitySprite.y; i++)
            {
                var indexI = startPosition.y + i;
                for (int j = 0; j < _unitySprite.x; j++)
                {
                    var inkecJ = startPosition.x + j;
                    var color = texture.GetPixel(indexI, inkecJ);
                    var term = DefineState(color, new Vector2Int(i, j));
                    if (term != null)
                    {
                        yield return term;
                    }
                }
            }
        }
        private HandTermTile DefineState(Color color, Vector2Int termPosition)
        {
            if (color == deactiveColor)
                return null;
            if (!Equale(color, _deffaout, errorColorDefaout))
            {
                var term = termArray[termPosition.x, termPosition.y];
                term.Activate(FireState.Start);
                return term;
            }
            return null;
        }
        #endregion
        private void DeactivePreviusTils(List<HandTermTile> curret, List<HandTermTile> previous)
        {
            if (previous == null)
                return;
            var count = previous.Count();
            for (int i = 0; i < count; i++)
            {
                if (!curret.Contains(previous[i]))
                {
                    previous[i].Deactivate();
                }
            }
        }

        private bool Equale(Color to, Color from, float errorFiltr = 0f)
        {
            var b = Mathf.Abs(to.b - from.b);
            var g = Mathf.Abs(to.g - from.g);
            var r = Mathf.Abs(to.r - from.r);
            return r < errorFiltr && b < errorFiltr && g < errorFiltr;
        }

        private Vector2Int GetCountOffset()
        {
            var x = _spriteAtlas.texture.width / _unitySprite.x;
            var y = _spriteAtlas.texture.height / _unitySprite.y;
            return new Vector2Int(x, y);
        }
    }
}