using System.Collections.Generic;

namespace Underworld
{
    public class PatternStateSwithcer : IStateSwitcher
    {
        private List<IPatternState> _states = new List<IPatternState>();

        public bool AddState(IPatternState state)
        {
            if (!_states.Contains(state))
            {
                _states.Add(state);
                return true;
            }
            return false;
        }
        public bool Remove(IPatternState state)
        {
            return _states.Remove(state);
        }
        public bool SwitchState<T>(out IPatternState result) where T : IPatternState
        {
            foreach (var state in _states)
            {
                if (state is T value)
                {
                    result = value;
                    result.Start();
                    return true;
                }
            }
            result = default(T);
            return false;
        }
    }
}
