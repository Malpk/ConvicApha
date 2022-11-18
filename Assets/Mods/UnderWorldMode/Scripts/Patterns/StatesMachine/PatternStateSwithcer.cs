using System.Collections.Generic;

namespace Underworld
{
    public class PatternStateSwithcer : IStateSwitcher
    {
        private List<BasePatternState> _states = new List<BasePatternState>();

        public bool AddState(BasePatternState state)
        {
            if (!_states.Contains(state))
            {
                _states.Add(state);
                return true;
            }
            return false;
        }
        public bool Remove(BasePatternState state)
        {
            return _states.Remove(state);
        }
        public bool SwitchState<T>(out BasePatternState result) where T : BasePatternState
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
