using UnityEngine;

namespace Underworld
{
    public class PatternLinerInterplate : BasePatternState
    {
        private readonly float duration;

        private float _progress;

        public PatternLinerInterplate(float duration)
        {
            this.duration = duration;
        }

        public System.Action<float> OnUpdate;

        public override bool IsComplite => _progress >= 1f;

   
        public override void Start()
        {
            _progress = 0f;
        }
        public override bool Update()
        {
            _progress += Time.deltaTime / duration;
            OnUpdate?.Invoke(_progress);
            return _progress < 1f;
        }
    }
}