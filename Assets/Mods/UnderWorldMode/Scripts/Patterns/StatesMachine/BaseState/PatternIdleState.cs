using UnityEngine;

namespace Underworld
{
    public class PatternIdleState : BasePatternState
    {
        private readonly float duration;

        private float _progress = 0f;

        public System.Action OnComplite;

        public PatternIdleState(float duration)
        {
            this.duration = duration;
        }

        public override bool IsComplite => _progress >= 1f;

        public override void Start()
        {
            _progress = 0f;
        }
        public override bool Update()
        {
            _progress += Time.deltaTime / duration;
            return _progress < 1f;
        }
    }
}