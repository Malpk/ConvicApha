using UnityEngine;

namespace Underworld
{
    public class PatternLinerInterplate<T> : IPatternState where T : IPatternState
    {
        private readonly float duration;
        private readonly IStateSwitcher switcher;

        private float _progress;

        public PatternLinerInterplate(IStateSwitcher switcher, float duration)
        {
            this.duration = duration;
            this.switcher = switcher;
        }

        public System.Action<float> OnUpdate;

        public bool IsComplite => _progress >= 1f;

   
        public void Start()
        {
            _progress = 0f;
        }
        public bool Update()
        {
            _progress += Time.deltaTime / duration;
            OnUpdate?.Invoke(_progress);
            return _progress < 1f;
        }
        public bool SwitchState(out IPatternState nextState)
        {
           return switcher.SwitchState<T>(out nextState);
        }
    }
}