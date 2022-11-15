using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class PatternCreaterParceState : IPatternState
    {
        private readonly float delay;
        private readonly Vector2Int countOffset;
        private readonly IStateSwitcher switcher;

        private int XOffset = 0;
        private int YOffset = 0;
        private float _progress = 0f;

        public System.Action<Vector2Int> OnUpdateFrame;

        public bool IsComplite => YOffset >= countOffset.y;

        public PatternCreaterParceState(IStateSwitcher switcher,float delay, Vector2Int countOffset)
        {
            this.delay = delay;
            this.switcher = switcher;
            this.countOffset = countOffset;
        }
        public void Start()
        {
            _progress = 0f;
            XOffset = 0;
            YOffset = 0;
        }

        public bool Update()
        {
            _progress += Time.deltaTime / delay;
            if(_progress >= 1)
            {
                if (XOffset == countOffset.x)
                {
                    XOffset = 0;
                    YOffset++;
                }
                OnUpdateFrame?.Invoke(new Vector2Int(XOffset, YOffset));
                XOffset++;
            }
            return YOffset < countOffset.y;
        }
        public bool SwitchState(out IPatternState nextState)
        {
            return switcher.SwitchState<TotalMapCompliteState>(out nextState);
        }

    }
}
