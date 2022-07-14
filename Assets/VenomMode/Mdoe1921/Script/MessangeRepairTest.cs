using UnityEngine;
using UnityEngine.UI;

namespace MainMode.Mode1921
{
    [System.Serializable]
    public class MessangeRepairTest
    {
        [SerializeField] private string _messange;
        [SerializeField] private Color _textColor;
        [SerializeField] private Image _colorArea;
        [SerializeField] private TypeResult _typeResult;
        
        public float Widht => _colorArea.rectTransform.rect.width/2;
        public float ReduceValue => GetReduceValue(_typeResult);
        public string Messange => _messange;
        public Color TextColor => _textColor;

        private float GetReduceValue(TypeResult result)
        {
            switch (result)
            {
                case TypeResult.Fail:
                    return 0.5f;
                case TypeResult.PartSucces:
                    return 0.2f;
            }
            return 0f;
        }
    }
}