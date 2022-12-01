using UnityEngine;

namespace Underworld
{
    [System.Serializable]
    public class Shape
    {
        [SerializeField] private string _name = "nameShape";
        [SerializeField] private Vector2Int _sizeShape;
        [SerializeField] private Vector2Int _startPixel;

        private Vector2Int _end;
        private TermMode[,] _shape;

        public TermMode[,] ShapeMap => _shape;

        public bool CreateShape(Texture2D texture, bool inversMode = false)
        {
            if (_shape != null)
                return false;
            var sizeTexture = new Vector2Int(texture.width, texture.height);
            ClampVector(sizeTexture);
            _shape = ReadTexture(inversMode,texture);
            return true;
        }
        private TermMode[,] ReadTexture(bool inversMode,Texture2D texture)
        {
            var activeTile = inversMode ? Color.black : Color.white;
            var shape = new TermMode[_sizeShape.y, _sizeShape.x];
            for (int i = 0; i <  _sizeShape.y; i++)
            {
                for (int j = 0; j < _sizeShape.x; j++)
                {
                    shape[i, j] = texture.GetPixel(_startPixel.x + j, _startPixel.y + i) == activeTile ?
                        TermMode.AttackMode : TermMode.Deactive;
                }
            }
            return shape;
        }
        private void ClampVector(Vector2Int tetureSize)
        {
            _sizeShape.Clamp(Vector2Int.zero, tetureSize);
            _startPixel.Clamp(Vector2Int.zero, tetureSize);
            _end = _startPixel + _startPixel;
            _end.Clamp(Vector2Int.zero, tetureSize);
        }
    }
}