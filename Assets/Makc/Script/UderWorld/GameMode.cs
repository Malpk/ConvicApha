using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Underworld
{
    public abstract class GameMode : MonoBehaviour
    {
        public abstract bool statusWork { get; }

        private void Update()
        {
            ModeUpdate();
        }
        protected abstract void ModeUpdate();
    }
}