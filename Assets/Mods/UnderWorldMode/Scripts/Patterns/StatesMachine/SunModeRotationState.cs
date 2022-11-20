using UnityEngine;

namespace Underworld
{
    public class SunModeRotationState : BasePatternState
    {
        private readonly int countRotation;
        private readonly float delay;
        private readonly float durationRotation;

        private int _count = 0;
        private float _progress = 0f;
        private float _delayProgress = 0f;

        private System.Func<bool> curretTask;

        public SunModeRotationState(int countRotation, float durationRotation, float delay)
        {
            this.delay = delay;
            this.countRotation = countRotation;
            this.durationRotation = durationRotation;
        }

        public System.Action<float> OnUpdate;
        public System.Action OnReset;
        
        public override bool IsComplite => _count >= countRotation;

        public override void Start()
        {
            _count = 0;
            Reset();
            curretTask = Delay;
        }
        public override bool Update()
        {
            if (!curretTask())
            {
                if (Switch(out System.Func<bool> task))
                {
                    curretTask = task;
                }
                else
                {
                    _count++;
                    Reset();
                    Switch(out curretTask);
                }
            }
            return _count < countRotation; 
        }
        private void Reset()
        {
            _progress = 0f;
            _delayProgress = 0f;
            OnReset?.Invoke();
        }
        private bool Delay()
        {
            _delayProgress += Time.deltaTime / delay;
            return _delayProgress < 1f;
        }
        private bool RotateRays()
        {
            _progress += Time.deltaTime / durationRotation;
            OnUpdate?.Invoke(_progress);
            return _progress < 1f;
        }
        private bool Switch(out System.Func<bool> task)
        {
            task = null;
            if (_delayProgress < 1f)
            {
                task = Delay;
            }
            else if (_progress < 1f)
            {
                task = RotateRays;
            }
            return task != null;
        }
    }
}