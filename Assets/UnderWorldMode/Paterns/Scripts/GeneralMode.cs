using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Underworld
{
    public abstract class GeneralMode : MonoBehaviour, IPause
    {
        [Header("General Setting")]
#if UNITY_EDITOR
        [SerializeField] protected bool isDebug;
#endif
        [SerializeField] protected bool playOnStart;
        [SerializeField] protected float workDuration;

        private ModeState _previus;
        public abstract bool IsReady { get; }
        public ModeState State { get; protected set; } = ModeState.Stop;

        protected virtual async void Awake()
        {
            await LoadAsync();
        }
        protected void OnDestroy()
        {
            Unload();
        }
        public abstract void Intializate(MapBuilder builder, Player player);
        protected abstract Task<bool> LoadAsync();
        protected abstract void Unload();
        public abstract bool Activate();

        public void Deactivate()
        {
            State = ModeState.Stop;
            Unload();
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