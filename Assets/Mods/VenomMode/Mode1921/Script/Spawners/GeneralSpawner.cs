using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using System;

namespace MainMode.Mode1921
{
    public abstract class GeneralSpawner : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnStart;
        [Header("General Reference")]
        [SerializeField] protected MapGrid mapGrid;

        protected event Action PlayAction;
        protected event Action StopAction;

        public bool IsPaly { private set; get; }

        public void Intializate(MapGrid mapGrid)
        {
            this.mapGrid = mapGrid;
        }

        private void Start()
        {
            if (_playOnStart)
                Play();
        }

        public void Play()
        {
            if (!IsPaly)
            {
                IsPaly = true;
                if (PlayAction != null)
                    PlayAction();
            }
        }
        public void Stop()
        {
            if (IsPaly)
            {
                IsPaly = false;
                if (StopAction != null)
                    StopAction();
            }
        }

        public abstract void Replay();

    }
}