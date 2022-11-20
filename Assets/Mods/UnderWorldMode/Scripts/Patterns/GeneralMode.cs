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

        public bool IsPlay { get; private set; }

        public abstract void SetConfig(PaternConfig config);
        public abstract void Intializate(MapBuilder builder, Player player);
        public void Play()
        {
            IsPlay = true;
            PlayMode();
        }

        public void Stop()
        {
            IsPlay = false;
            StopMode();
        }

        protected abstract void PlayMode();
        protected abstract void StopMode();
    }
}