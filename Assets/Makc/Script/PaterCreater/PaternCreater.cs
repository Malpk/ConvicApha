using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchModeComponent;
using System.Linq;

namespace Underworld
{
    public class PaternCreater : MonoBehaviour,IModeForSwitch
    {
        [Header("Game setting")]
        [SerializeField] protected float changeSpeed;
        [SerializeField] protected bool iversionMode;
        [Header("Frame Setting")]
        [SerializeField] protected float errorColorDefaout;
        [SerializeField] protected Color _deffaout;
        [SerializeField] protected Vector2Int _unitySprite;
        [SerializeField] protected Sprite _spriteAtlas;
       
        protected int oneSecond = 1;
        protected Color deactiveColor;
        protected Point[,] _map;

        public bool IsAttackMode => true;

        private void Awake()
        {
            deactiveColor = iversionMode ? Color.black : Color.white;
        }
        public void Constructor(SwitchMode swictMode)
        {
            _map = swictMode.builder.Map;
            StartCoroutine(RunPatern());
        }
        private IEnumerator RunPatern()
        {
            var countOffset = GetCountOffset();
            List<Point> previousFrame = null;
            for (int i = 0; i < countOffset.y; i++)
            {
                for (int j = 0; j < countOffset.x; j++)
                {
                    var curretPosition = new Vector2Int(i * _unitySprite.y, j * _unitySprite.y);
                    var curretFrame = ReadTexture(_spriteAtlas.texture, curretPosition).ToList();
                    DeactivePreviusTils(curretFrame, previousFrame);
                    previousFrame = curretFrame;
                    yield return new WaitForSeconds(oneSecond / changeSpeed);
                }
            }
            Destroy(gameObject);
        }
        private IEnumerable<Point> ReadTexture(Texture2D texture, Vector2Int startPosition)
        {
            for (int i = 0; i < _unitySprite.y; i++)
            {
                var indexI = startPosition.y + i;
                for (int j = 0; j < _unitySprite.x; j++)
                {
                    var inkecJ = startPosition.x + j;
                    var color = texture.GetPixel(indexI, inkecJ);
                    var point = DefineState(color, new Vector2Int(i, j));
                    if (point != null)
                    {
                        yield return point;
                    }
                }
            }
        }
        private void DeactivePreviusTils(List<Point> curret, List<Point> previous)
        {
            if (previous == null)
                return;
            var count = previous.Count();
            for (int i = 0; i < count; i++)
            {
                var point = previous[i];
                if (!curret.Contains(point))
                {
                    point.SetAtiveObject(false);
                    point.Animation.Stop();
                }
            }
        }
        private Point DefineState(Color color,Vector2Int arrayPosition)
        {
            if (color == deactiveColor)
                return null;
            var point = _map[arrayPosition.x, arrayPosition.y];
            if(!point.IsActive)
                point.SetAtiveObject(true);
            if (!Equale(color, _deffaout, errorColorDefaout))
            {
                point.Animation.StartTile();
            }
            return point;
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