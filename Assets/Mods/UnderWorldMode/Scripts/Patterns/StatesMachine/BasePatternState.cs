namespace Underworld
{
    public abstract class BasePatternState
    {
        private BasePatternState _nexSstate;

        
        public System.Action OnComplite;
        
        public abstract bool IsComplite { get; }
      

        public void SetNextState(BasePatternState nextState)
        {
            _nexSstate = nextState;
        }

        public abstract void Start();
        public abstract bool Update();

        public bool GetNextState(out BasePatternState nextState)
        {
            if (_nexSstate != null)
            {
                nextState = _nexSstate;
                nextState.Start();
                OnComplite?.Invoke();
                return true;
            }
            nextState = null;
            return false;
        }
    }
}