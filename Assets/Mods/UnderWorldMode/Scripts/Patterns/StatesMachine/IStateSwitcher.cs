namespace Underworld
{
    public interface IStateSwitcher
    {
        public bool AddState(IPatternState state);
        public bool SwitchState<T>(out IPatternState result) where T : IPatternState;
    }
}