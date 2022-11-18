namespace Underworld
{
    public class SunModeRotationState : BasePatternState
    {
        private readonly int countRotation;
        private readonly PatternLinerInterplate rotateDuration;
        private readonly PatternIdleState delayState;

        private int _count = 0;
        private BasePatternState _curretState;

        public SunModeRotationState(int countRotation, float durationRotation, float delay)
        {
            this.countRotation = countRotation;
            rotateDuration = new PatternLinerInterplate(durationRotation);
            delayState = new PatternIdleState(delay);
            delayState.SetNextState(rotateDuration);
        }

        public override bool IsComplite => _count >= countRotation;

        public override void Start()
        {
            _curretState = delayState;
            _count = 0;

        }
        public override bool Update()
        {
            if (_curretState.IsComplite)
            {
                Switch();
            }
            else
            {
                _curretState.Update();
            }
            return _count < countRotation; 
        }
        private void Switch()
        {
            if (_curretState.GetNextState(out BasePatternState state))
            {
                _curretState = state;
            }
            else
            {
                _curretState = delayState;
                _count++;
            }
        }
    }
}