using UnityEngine;

namespace Underworld
{
    public class PatternCreaterParceState : BasePatternState
    {
        private readonly float delay;
        private readonly Vector2Int countOffset;

        private int XOffset = 0;
        private int YOffset = 0;
        private float _progress = 0f;

        public System.Action<Vector2Int> OnUpdateFrame;

        public override bool IsComplite => YOffset >= countOffset.y;

        public PatternCreaterParceState(float delay, Vector2Int countOffset)
        {
            this.delay = delay;
            this.countOffset = countOffset;
        }
        public override void Start()
        {
            _progress = 0f;
            XOffset = 0;
            YOffset = 0;
        }

        public override bool Update()
        {
            _progress += Time.deltaTime / delay;
            if(_progress >= 1)
            {
                if (XOffset == countOffset.x)
                {
                    XOffset = 0;
                    YOffset++;
                }
                if (YOffset < countOffset.y)
                {
                    OnUpdateFrame?.Invoke(new Vector2Int(XOffset, YOffset));
                    XOffset++;
                    _progress = 0;
                }
            }
            return YOffset < countOffset.y;
        }
    }
}
