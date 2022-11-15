using UnityEngine;

namespace Underworld
{
    public class PatternIdleState<T> : IPatternState where T : IPatternState
    {
        private readonly float duration;
        private readonly IStateSwitcher switcher;

        private float _progress = 0f;

        public System.Action OnComplite;

        public PatternIdleState(IStateSwitcher switcher, float duration)
        {
            this.duration = duration;
            this.switcher = switcher;
        }

        public bool IsComplite => _progress >= 1f;

        public void Start()
        {
            _progress = 0f;
        }
        public bool Update()
        {
            _progress += Time.deltaTime / duration;
            return _progress < 1f;
        }
        public bool SwitchState(out IPatternState nextState)
        {
            OnComplite?.Invoke();
            return switcher.SwitchState<T>(out nextState);
        }
    }
}