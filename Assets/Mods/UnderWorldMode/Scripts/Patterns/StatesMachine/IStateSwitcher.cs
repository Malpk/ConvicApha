namespace Underworld
{
    public interface IStateSwitcher
    {
        public bool AddState(BasePatternState state);
        public bool SwitchState<T>(out BasePatternState result) where T : BasePatternState;
    }
}