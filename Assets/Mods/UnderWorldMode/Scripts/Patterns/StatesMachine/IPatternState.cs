namespace Underworld
{
    public interface IPatternState
    {
        public bool IsComplite { get; }

        public void Start();
        public bool Update();

        public bool SwitchState(out IPatternState nextState);
    }
}