using System.Collections;
using UnityEngine;
using System;

namespace Underworld
{
    public abstract class GeneralMode : MonoBehaviour, IPause
    {
        [Header("General Setting")]
        [SerializeField] protected bool playOnStart;
        [Min(0)]
        [SerializeField] protected float workDuration;

        protected PatternStateSwithcer switcher = new PatternStateSwithcer();
        
        protected event Action DeactivateAction;

        private ModeState _previus;

        public ModeState State { get; protected set; } = ModeState.Stop;

        public abstract void SetConfig(PaternConfig config);
        public abstract void Intializate(MapBuilder builder, Player player);
        public abstract bool Play();

        public void Deactivate()
        {
            State = ModeState.Stop;
            if (DeactivateAction != null)
                DeactivateAction();
        }
        public virtual void Pause()
        {
            _previus = State;
            State = ModeState.Pause;
        }
        public virtual void UnPause()
        {
            State = _previus;
        }
        protected IEnumerator WaitTime(float duration)
        {
            var progress = 0f;
            while (progress <= 1f)
            {
                yield return new WaitWhile(() => State == ModeState.Pause);
                yield return null;
                progress += Time.deltaTime / duration;
            }
        }
    }
}