using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SwitchModeComponent;

namespace Underworld
{
    public class PaternCreater : MonoBehaviour,ISequence
    {
        [Header("Game setting")]
        [SerializeField] protected float changeSpeed;
        [SerializeField] protected bool iversionMode;
        [Header("Perfab setting")]
        [SerializeField] protected float maxDiferentForColor;
        [SerializeField] protected Color _deffaout;
        [SerializeField] protected Vector2Int _unitySprite;
        [SerializeField] protected Sprite _spriteAtlas;
       
        [Inject] protected MapBuilder mapBuilder;
        [Inject] protected GameMap _map;

        protected int oneSecond = 1;
        protected Color _turnTileColor;
        protected IRead reader;

        public bool IsAttackMode => true;

        private void Awake()
        {
            _turnTileColor = iversionMode ? Color.black : Color.white;
        }
        public void Constructor(SwitchMods swictMode)
        {
            _map = swictMode.map;
            mapBuilder = swictMode.builder;
            mapBuilder.Intialiate(_map);
            StartCoroutine(RunPatern());
        }

        private void OnDestroy()
        {
            mapBuilder.OnDestroy();
        }
        private IEnumerator RunPatern()
        {
            var countOffset = GetCountOffset();
            for (int i = 0; i < countOffset.y; i++)
            {
                for (int j = 0; j < countOffset.x; j++)
                {
                    var curretPosition = new Vector2Int(i * _unitySprite.y, j * _unitySprite.y);
                    var nap = ReadTexture(_spriteAtlas.texture, curretPosition);
                    mapBuilder.MapUpdate(nap);
                    yield return new WaitForSeconds(oneSecond / changeSpeed);
                }
            }
            mapBuilder.TurnOffAllTile();
            Destroy(gameObject);
        }

        private TermMode [,] ReadTexture(Texture2D texture, Vector2Int startPosition)
        {
            var modeMap = new TermMode[_unitySprite.y, _unitySprite.x];
            for (int i = 0; i < _unitySprite.y; i++)
            {
                for (int j = 0; j < _unitySprite.x; j++)
                {
                    var color = texture.GetPixel(startPosition.y + i, startPosition.x + j);
                    modeMap[_unitySprite.y-1 - i, _unitySprite.x - 1 - j] = DefineMode(color);
                }
            }
            return modeMap;
        }
        private TermMode DefineMode(Color color)
        {
            if (Equale(color, _deffaout, maxDiferentForColor))
            {
                return TermMode.SafeMode;
            }
            else if (color == _turnTileColor)
            {
                return TermMode.AttackMode;
            }
            else
            {
                return TermMode.Deactive;
            }
        }
        private bool Equale(Color to, Color from, float doorstep = 0f)
        {
            var b = Mathf.Abs(to.b - from.b);
            var g = Mathf.Abs(to.g - from.g);
            var r = Mathf.Abs(to.r - from.r);
            return r < doorstep && b < doorstep && g < doorstep;
        }

        private Vector2Int GetCountOffset()
        {
            var x = _spriteAtlas.texture.width / _unitySprite.x;
            var y = _spriteAtlas.texture.height / _unitySprite.y;
            return new Vector2Int(x, y);
        }
    }
}