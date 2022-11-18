using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace Underworld
{
    public abstract class GeneralMode : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] protected bool playOnStart;
        [Min(0)]
        [SerializeField] protected float workDuration;

        protected event Action DeactivateAction;

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